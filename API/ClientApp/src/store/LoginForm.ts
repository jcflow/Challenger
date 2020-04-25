import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LoginFormState {
    isLoading: boolean;
    user: User | null;
}

export interface User {
    id: number;
    name: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
interface RequestCategoriesAction {
    type: 'REQUEST_LOGIN';
}

interface ReceiveCategoriesAction {
    type: 'RECEIVE_LOGIN';
    user: User;
}

interface LogOutAction {
    type: 'LOG_OUT';
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestCategoriesAction | ReceiveCategoriesAction | LogOutAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    logIn: (data: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.login) {
            const variables = {
                name: data.name,
                password: data.password
            };
            const query = 'mutation ($name:String!, $password:String!) { login(name: $name, password: $password) { id, name } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => res.json())
                .then(res => {
                    dispatch({ type: 'RECEIVE_LOGIN', user: res.data.login });
                    localStorage.setItem('user', JSON.stringify(res.data.login));
                });
            dispatch({ type: 'REQUEST_LOGIN' });
        }
    },
    logOut: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.login) {
            dispatch({ type: 'LOG_OUT' });
            localStorage.removeItem('user');
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const getUser = () => {
    // @ts-ignore
    var user = JSON.parse(localStorage.getItem('user'));
    return user;
};

const unloadedState: LoginFormState = { isLoading: false, user: getUser() };

export const reducer: Reducer<LoginFormState> = (state: LoginFormState | undefined, incomingAction: Action): LoginFormState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_LOGIN':
            return {
                ...state,
                user: null,
                isLoading: true
            };
        case 'RECEIVE_LOGIN':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                user: action.user,
                isLoading: false
            };
        case 'LOG_OUT':
            return {
                ...state,
                user: null,
                isLoading: true
            };
    }

    return state;
};
