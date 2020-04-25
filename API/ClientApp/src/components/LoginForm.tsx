import * as React from 'react';
import { connect } from 'react-redux';
import { Redirect } from "react-router-dom";
import { RouteComponentProps } from "react-router";
import { ApplicationState } from '../store';
import * as LoginFormStore from '../store/LoginForm';

// At runtime, Redux will merge together...
type LoginFormProps =
    LoginFormStore.LoginFormState // ... state we've requested from the Redux store
    & typeof LoginFormStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps; // ... plus incoming routing parameters

type LoginFormPropsState = {
    name: string,
    password: string
};

class LoginForm extends React.Component<LoginFormProps, LoginFormPropsState> {

    public constructor(props: any) {
        super(props);
        this.state = {
            name: "",
            password: ""
        };
    }

    private handleNameChange(e: any) {
        const name = e.currentTarget.value;
        this.setState({ name: name });
    }

    private handlePasswordChange(e: any) {
        const password = e.currentTarget.value;
        this.setState({ password: password });
    }

    private handleSubmit() {
        const data = {
            name: this.state.name,
            password: this.state.password
        };
        this.props.logIn(data);
    }

    private verifySubmit() {
        return this.state.name.trim() !== "" &&
            this.state.password.trim() !== "";
    }

    public render() {
        if (this.props.user) {
            return <Redirect to="/" />
        }

        const canSubmit = this.verifySubmit();

        return (
            <React.Fragment>
                <div className="title">
                    <h1>Log In</h1>
                </div>
                <div className="tournament-form swing-in-top-fwd">
                    <label htmlFor="name">Name:</label>
                    <input type="text" id="name" name="name" placeholder="Enter the user name..." onChange={(e) => this.handleNameChange(e)} value={this.state.name} />

                    <label htmlFor="password">Password:</label>
                    <input type="password" id="password" name="password" placeholder="Enter the user password..." onChange={(e) => this.handlePasswordChange(e)} value={this.state.password} />
                    <button className={`button green${canSubmit ? " jello-horizontal" : ""}`} disabled={!canSubmit} onClick={() => this.handleSubmit()}>Enter!</button>
                </div>
            </React.Fragment>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.login, // Selects which state properties are merged into the component's props
    LoginFormStore.actionCreators // Selects which action creators are merged into the component's props
)(LoginForm as any);
