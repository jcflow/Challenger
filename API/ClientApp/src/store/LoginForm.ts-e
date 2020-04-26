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
interface RequestLoginAction {
    type: 'REQUEST_LOGIN';
}

interface ReceiveLoginAction {
    type: 'RECEIVE_LOGIN';
    user: User;
}

interface RequestSignupAction {
    type: 'REQUEST_SIGNUP';
}

interface ReceiveSignupAction {
    type: 'RECEIVE_SIGNUP';
    user: User;
}

interface LogOutAction {
    type: 'LOG_OUT';
}

const loginProcess = (dispatch: Function, user: User) => {
    dispatch({ type: 'RECEIVE_SIGNUP', user: user });
    localStorage.setItem('user', JSON.stringify(user));
};

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestLoginAction | ReceiveLoginAction | RequestSignupAction | ReceiveSignupAction | LogOutAction;

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
                    if (!(res.data && res.data.login)) {
                        throw new Error("Could not log in.");
                    }
                    loginProcess(dispatch, res.data.login);
                })
                .catch(console.error);
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
    },
    signUp: (data: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.login) {
            const variables = {
                name: data.name,
                password: data.password
            };
            const query = 'mutation ($name:String!, $password:String!) { signup(name: $name, password: $password) { id, name } }';
            fetch('https://localhost:5001/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => res.json())
                .then(res => {
                    if (!(res.data && res.data.signup)) {
                        throw new Error("Could not sign up.");
                    }
                    loginProcess(dispatch, res.data.signup);
                })
                .catch(console.error);
            dispatch({ type: 'REQUEST_SIGNUP' });
        }
    },
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
        case 'REQUEST_SIGNUP':
            return {
                ...state,
                user: null,
                isLoading: true
            };
        case 'RECEIVE_LOGIN':
        case 'RECEIVE_SIGNUP':
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
