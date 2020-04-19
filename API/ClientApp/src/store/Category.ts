import  { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import {Tournament} from "./Tournament";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CategoryState {
    isLoading: boolean;
    tournaments: Tournament[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTournamentsAction {
    type: 'REQUEST_TOURNAMENTS';
}

interface ReceiveTournamentsAction {
    type: 'RECEIVE_TOURNAMENTS';
    tournaments: Tournament[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTournamentsAction | ReceiveTournamentsAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTournamentsByCategoryId: (id: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.category) {
            const variables = { id: id };
            const query = 'query($id: Int!) { category(id: $id) { tournaments { id name } } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => res.json())
                .then(res => {
                    dispatch({ type: 'RECEIVE_TOURNAMENTS', tournaments: res.data.category.tournaments });
                });
            dispatch({ type: 'REQUEST_TOURNAMENTS' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: CategoryState = { isLoading: false, tournaments: [] };

export const reducer: Reducer<CategoryState> = (state: CategoryState | undefined, incomingAction: Action): CategoryState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_TOURNAMENTS':
            return {
                ...state,
                isLoading: true
            };
        case 'RECEIVE_TOURNAMENTS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                tournaments: action.tournaments,
                isLoading: false
            };
    }

    return state;
};
