import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TournamentState {
    isLoading: boolean;
    id?: number;
    tournament?: Tournament;
}

export interface Tournament {
    id: number;
    name: string;
    brackets: Bracket[];
}

export interface Bracket {
    id: number;
    level: number;
    finished: boolean;
    scores: Score[];
}

export interface Score {
    id: number;
    value: number;
    team: Team;
}

export interface Team {
    id: number;
    name: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTournamentAction {
    type: 'REQUEST_TOURNAMENT';
}

interface ReceiveTournamentAction {
    type: 'RECEIVE_TOURNAMENT';
    tournament: Tournament;
}

interface UpdateBracketAction {
    type: 'UPDATE_BRACKET';
}

interface UpdateScoreAction {
    type: 'UPDATE_SCORE';
}

const requestTournament = (id: number) => {
    const variables = { id: id };
    const query = 'query($id: Int!) { tournament(id: $id) { id, name brackets { id level finished scores { id value team { id name } } } } }';
    return fetch('https://localhost:5001/graphql', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ query: query, variables: variables }),
    }).then(res => res.json());
};

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTournamentAction | ReceiveTournamentAction | UpdateBracketAction | UpdateScoreAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTournament: (id: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.tournament) {
            requestTournament(id)
                .then(res => {
                    dispatch({ type: 'RECEIVE_TOURNAMENT', tournament: res.data.tournament });
                });
            dispatch({ type: 'REQUEST_TOURNAMENT' });
        }
    },
    closeBracket: (id: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        const variables = { id: id, finished: true };
        if (appState && appState.tournament) {
            const query = 'mutation ($id: Int!, $finished: Boolean!) { updateBracket(id: $id, finished: $finished) { id level finished } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => requestTournament(appState.tournament!.tournament!.id!))
                .then(res => {
                    // @ts-ignore
                    dispatch({ type: 'RECEIVE_TOURNAMENT', tournament: res.data.tournament });
                });
            dispatch({ type: 'UPDATE_BRACKET' });
        }
    },
    updateScore: (id: number, value: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        const variables = { id: id, value: value };
        if (appState && appState.tournament) {
            const query = 'mutation ($id: Int!, $value: Int!) { updateScore(id: $id, value: $value) { id value } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => requestTournament(appState.tournament!.tournament!.id))
                .then(res => {
                    // @ts-ignore
                    dispatch({ type: 'RECEIVE_TOURNAMENT', tournament: res.data.tournament });
                });
            dispatch({ type: 'UPDATE_SCORE' });
        }
    },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TournamentState = { isLoading: false };

export const reducer: Reducer<TournamentState> = (state: TournamentState | undefined, incomingAction: Action): TournamentState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_TOURNAMENT':
            return {
                ...state,
                isLoading: true
            };
        case 'RECEIVE_TOURNAMENT':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                tournament: action.tournament,
                isLoading: false
            };
        case 'UPDATE_BRACKET':
            return {
                ...state,
                isLoading: true,
            };
    }

    return state;
};
