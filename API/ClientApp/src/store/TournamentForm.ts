import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import { Category } from './Home';
// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TournamentFormState {
    isLoading: boolean;
    id?: number;
    categories: Category[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
interface CleanFormAction {
    type: 'CLEAN_FORM';
}

interface RequestCategoriesAction {
    type: 'REQUEST_CATEGORIES';
}

interface ReceiveCategoriesAction {
    type: 'RECEIVE_CATEGORIES';
    categories: Category[];
}

interface PostTournamentAction {
    type: 'POST_TOURNAMENT';
}

interface NavigateTournamentAction {
    type: 'NAVIGATE_TOURNAMENT';
    id: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = CleanFormAction | RequestCategoriesAction | ReceiveCategoriesAction| PostTournamentAction | NavigateTournamentAction;

// ----------------
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    postTournament: (data: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.tournamentForm) {
            const variables = {
                tournament: {
                    name: data.name
                },
                teams: data.teamNames.map((teamName: string) => ({ name: teamName })),
                categoryId: data.categoryId,
                userId: data.userId
            };

            console.log(data);
            console.log(variables);
            const query = 'mutation ($tournament:tournamentInput!, $teams:[teamInput]!, $categoryId:Int!, $userId:Int!) { createTournament(tournament: $tournament, teams: $teams, categoryId: $categoryId, userId: $userId) { id, name } }';
            fetch('/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ query: query, variables: variables }),
            })
                .then(res => res.json())
                .then(res => {
                    if (!(res.data && res.data.createTournament)) {
                        throw new Error("Could not create tournament.");
                    }
                    dispatch({ type: 'NAVIGATE_TOURNAMENT', id: res.data.createTournament.id });
                })
                .catch(console.error);
            dispatch({ type: 'POST_TOURNAMENT' });
        }
    },
    requestCategories: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.tournamentForm) {
            const query = 'query { categories { id name } }';
            fetch('/graphql', {
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
                })
                .catch(console.error);
            dispatch({ type: 'REQUEST_CATEGORIES' });
        }
    },
    cleanForm: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.tournamentForm) {
            dispatch({ type: 'CLEAN_FORM' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TournamentFormState = { isLoading: false, categories: [] };

export const reducer: Reducer<TournamentFormState> = (state: TournamentFormState | undefined, incomingAction: Action): TournamentFormState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_CATEGORIES':
            return {
                ...state,
                id: undefined,
                isLoading: true
            };
        case 'RECEIVE_CATEGORIES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                ...state,
                id: undefined,
                categories: action.categories,
                isLoading: false
            };
        case 'POST_TOURNAMENT':
            return {
                ...state,
                id: undefined,
                isLoading: true
            };
        case 'NAVIGATE_TOURNAMENT':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                id: action.id,
                isLoading: false,
                categories: []
            };
        case 'CLEAN_FORM':
            return {
                ...state,
                id: undefined,
                isLoading: true
            };
    }

    return state;
};
