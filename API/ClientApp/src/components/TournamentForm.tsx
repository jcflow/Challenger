// @ts-nocheck
import _ from 'lodash';
import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps, Redirect } from 'react-router';
import { ApplicationState } from '../store';
import * as TournamentFormStore from '../store/TournamentForm';
import { ListGroup, ListGroupItem, FormGroup, Button, Input, Label, Form } from 'reactstrap';

// At runtime, Redux will merge together...
type TournamentFormProps =
    TournamentFormStore.TournamentFormState // ... state we've requested from the Redux store
    & typeof TournamentFormStore.actionCreators; // ... plus action creators we've requested

type TournamentFormPropsState = { name: string, currentTeamName: string, teamNames: string[] };

class TournamentForm extends React.PureComponent<TournamentFormProps, TournamentFormPropsState> {

    public constructor(props: any) {
        super(props);
        this.state = {
            name: "",
            currentTeamName: "",
            teamNames: []
        };
    }

    private handleNameBlur(e: any) {
        const name = e.currentTarget.value;
        this.setState({ name: name });
    }

    private handleTeamNameBlur(e: any) {
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
        // Workaround
        // @ts-ignore
        document.getElementById("newTeam").value = "";
    }

    private handleSubmit() {
        const data = {
            name: this.state.name,
            teamNames: this.state.teamNames
        };
        this.props.postTournament(data);
    }

    public render() {
        if (this.props.id > 0) {
            return <Redirect to={"/tournament/" + this.props.id} />
        }
        const canSubmit = this.state.teamNames.length >= 2 && Number.isInteger(Math.log2(this.state.teamNames.length));

        return (
            <React.Fragment>
                <h1>Create a new tournament!</h1>
                <Form className="tournamentForm">
                    <input type="text" id="tournamentName" placeholder="Enter the tournament name..." onBlur={(e) => this.handleNameBlur(e)} />
                    <h2>Teams</h2>
                    <div className="dividedField">
                        <input type="text" name="newTeam" id="newTeam" placeholder="Enter a new team..."
                               onChange={(e) => this.handleTeamNameBlur(e)}
                               onKeyUp={(e) => {
                                   if (e.keyCode === 13) {
                                       e.preventDefault();
                                       this.handleAddTeamClick(e);
                                   }
                               }} />
                        <Button color="success" onClick={(e) => this.handleAddTeamClick(e)}>+</Button>
                    </div>
                    <ListGroup>
                        {
                            this.state.teamNames.map((teamName: string) => <ListGroupItem key={_.uniqueId(teamName)}>{teamName}</ListGroupItem>)
                        }
                    </ListGroup>
                    <Button disabled={!canSubmit} onClick={() => this.handleSubmit()}>Submit!</Button>
                </Form>
            </React.Fragment>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.tournamentForm, // Selects which state properties are merged into the component's props
    TournamentFormStore.actionCreators // Selects which action creators are merged into the component's props
)(TournamentForm as any);
