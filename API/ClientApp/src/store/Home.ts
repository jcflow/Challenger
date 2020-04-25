import  { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface HomeState {
    isLoading: boolean;
    categories: Category[];
}

export interface Category {
    id?: number
    name?: string
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestCategoriesAction {
    type: 'REQUEST_CATEGORIES';
}

interface ReceiveCategoriesAction {
    type: 'RECEIVE_CATEGORIES';
    categories: Category[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestCategoriesAction | ReceiveCategoriesAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestCategories: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.home) {
            const query = 'query { categories { id name } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query }),
            })
                .then(res => res.json())
                .then(res => {
                    if (!(res.data && res.data.categories)) {
                        throw new Error("Could not retrieve categories.");
                    }
                    dispatch({ type: 'RECEIVE_CATEGORIES', categories: res.data.categories });
                });
            dispatch({ type: 'REQUEST_CATEGORIES' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: HomeState = { isLoading: false, categories: [] };

export const reducer: Reducer<HomeState> = (state: HomeState | undefined, incomingAction: Action): HomeState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_CATEGORIES':
            return {
                ...state,
                isLoading: true
            };
        case 'RECEIVE_CATEGORIES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                categories: action.categories,
                isLoading: false
            };
    }

    return state;
};
