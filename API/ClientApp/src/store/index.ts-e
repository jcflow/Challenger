import * as Home from './Home';
import * as Tournament from './Tournament';
import * as Category from './Category';
import * as TournamentForm from './TournamentForm';
import * as LoginForm from './LoginForm';

// The top-level state object
export interface ApplicationState {
    home: Home.HomeState | undefined;
    login: LoginForm.LoginFormState | undefined;
    category: Category.CategoryState | undefined;
    tournament: Tournament.TournamentState | undefined;
    tournamentForm: TournamentForm.TournamentFormState | undefined;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    home: Home.reducer,
    login: LoginForm.reducer,
    category: Category.reducer,
    tournament: Tournament.reducer,
    tournamentForm: TournamentForm.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
