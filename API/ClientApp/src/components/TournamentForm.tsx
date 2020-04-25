// @ts-nocheck
import _ from 'lodash';
import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from "react-router";
import { ApplicationState } from '../store';
import * as TournamentFormStore from '../store/TournamentForm';
import * as LoginFormStore from '../store/LoginForm';

// At runtime, Redux will merge together...
type TournamentFormProps =
    TournamentFormStore.TournamentFormState // ... state we've requested from the Redux store
    & LoginFormStore.LoginFormState // ... state we've requested from the Redux store
    & typeof TournamentFormStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps; // ... plus incoming routing parameters

type TournamentFormPropsState = {
    name: string,
    categoryId?: number,
    currentTeamName: string,
    teamNames: string[]
};

class TournamentForm extends React.Component<TournamentFormProps, TournamentFormPropsState> {

    public constructor(props: any) {
        super(props);
        this.state = {
            name: "",
            currentTeamName: "",
            categoryId: undefined,
            teamNames: []
        };
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    private handleNameChange(e: any) {
        const name = e.currentTarget.value;
        this.setState({ name: name });
    }

    private handleTeamNameChange(e: any) {
        const currentTeamName = e.currentTarget.value;
        this.setState({ currentTeamName: currentTeamName });
    }

    private handleAddTeamClick(e: any) {
        e.preventDefault();
        if (!!this.state.currentTeamName.trim()) {
            var teamNames = this.state.teamNames;
            teamNames.push(this.state.currentTeamName);
            this.setState({ currentTeamName: "", teamNames: teamNames });
        }
    }

    private handleSubmit() {
        if (this.props.user !== null) {
            const data = {
                name: this.state.name,
                teamNames: this.state.teamNames,
                categoryId: this.state.categoryId,
                userId: this.props.user.id
            };
            this.props.postTournament(data);
        }
    }

    private handleCategoryChange(e: any) {
        this.setState({ categoryId: Number(e.target.value) });
    }

    public componentDidUpdate(prevProps: TournamentFormProps) {
        if (this.props.id) {
            const url = `/tournament/${this.props.id}`;
            this.props.cleanForm();
            this.props.history.push(url);
        }
        if (this.props.categories !== prevProps.categories && this.props.categories.length) {
            this.setState({ categoryId: this.props.categories[0].id });
        }
    }

    private verifySubmit() {
        return this.state.name.trim() !== "" &&
            this.state.teamNames.length >= 2 &&
            Number.isInteger(Math.log2(this.state.teamNames.length));
    }

    public render() {
        const canSubmit = this.verifySubmit();

        return (
            <React.Fragment>
                <div className="title">
                    <h1>Create a new tournament</h1>
                </div>
                <div className="tournament-form swing-in-top-fwd">
                    <label htmlFor="name">Name:</label>
                    <input type="text" id="name" name="name" placeholder="Enter the tournament name..." onChange={(e) => this.handleNameChange(e)} value={this.state.name} />

                    <label htmlFor="category">Category:</label>
                    <div className="select-wrapper">
                        <select id="category" name="category" placeholder="Select a category..." value={this.state.categoryId} onChange={(e) => this.handleCategoryChange(e)}>
                            {
                                this.props.categories.map((category) => <option key={category.id} value={category.id}>{category.name}</option>)
                            }
                        </select>
                    </div>

                    <label htmlFor="category">Teams:</label>
                    <form>
                        <input type="text" name="new-team" id="new-team" placeholder="Enter a new team..."
                            onChange={(e) => this.handleTeamNameChange(e)}
                            value={this.state.currentTeamName}
                        />
                        <input className="button" type="submit" onClick={(e) => this.handleAddTeamClick(e)} value="Add" />
                    </form>
                    {
                        (!!this.state.teamNames.length) &&
                            <ol className="item-list">
                                {
                                    this.state.teamNames.map((teamName: string) =>
                                        <li key={_.uniqueId(teamName)}>{teamName}</li>)
                                }
                            </ol>
                    }
                    <button className={`button green${canSubmit ? " jello-horizontal" : ""}`} disabled={!canSubmit} onClick={() => this.handleSubmit()}>Submit!</button>
                </div>
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        this.props.requestCategories();
    }
}

export default connect(
    (state: ApplicationState) => Object.assign({}, state.tournamentForm, state.login), // Selects which state properties are merged into the component's props
    TournamentFormStore.actionCreators // Selects which action creators are merged into the component's props
)(TournamentForm as any);
