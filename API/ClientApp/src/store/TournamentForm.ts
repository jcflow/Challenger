import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TournamentFormState {
    isLoading: boolean;
    name: string;
    id: number;
    teamNames: string[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface PostTournamentAction {
    type: 'POST_TOURNAMENT';
}

interface NavigateTournamentAction {
    type: 'NAVIGATE_TOURNAMENT';
    id: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = PostTournamentAction | NavigateTournamentAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    postTournament: (data: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        const variables = {
            tournament: {
                name: data.name
            },
            teams: data.teamNames.map((teamName: string) => ({ name: teamName }))
        };
        if (appState && appState.tournamentForm) {
            const query = 'mutation ($tournament:tournamentInput!, $teams:[teamInput]!) { createTournament(tournament: $tournament, teams: $teams) { id, name } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => res.json())
                .then(res => {
                    dispatch({ type: 'NAVIGATE_TOURNAMENT', id: res.data.createTournament.id });
                });
            dispatch({ type: 'POST_TOURNAMENT' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TournamentFormState = { isLoading: false, name: "", teamNames: [], id: 0 };

export const reducer: Reducer<TournamentFormState> = (state: TournamentFormState | undefined, incomingAction: Action): TournamentFormState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'POST_TOURNAMENT':
            return {
                ...state,
                isLoading: true
            };
        case 'NAVIGATE_TOURNAMENT':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                id: action.id,
                isLoading: false
            };
    }

    return state;
};
