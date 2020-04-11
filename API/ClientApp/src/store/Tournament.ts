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

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTournamentAction | ReceiveTournamentAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTournament: (id: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.tournament) {
            const query = '{ tournaments { id, name brackets { id level finished scores { id value team { id name } } } } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query }),
            })
                .then(res => res.json())
                .then(res => {
                    dispatch({ type: 'RECEIVE_TOURNAMENT', tournament: res.data.tournaments[0] });
                });
            dispatch({ type: 'REQUEST_TOURNAMENT' });
        }
    }
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
                isLoading: true
            };
        case 'RECEIVE_TOURNAMENT':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                tournament: action.tournament,
                isLoading: false
            };
    }

    return state;
};
