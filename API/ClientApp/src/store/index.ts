import * as Tournament from './Tournament';
import * as TournamentForm from './TournamentForm';

// The top-level state object
export interface ApplicationState {
    tournament: Tournament.TournamentState | undefined;
    tournamentForm: TournamentForm.TournamentFormState | undefined;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    tournament: Tournament.reducer,
    tournamentForm: TournamentForm.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
