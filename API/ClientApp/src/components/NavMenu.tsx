import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { ApplicationState } from '../store';
import { RouteComponentProps } from "react-router";
import * as LoginFormStore from '../store/LoginForm';

// At runtime, Redux will merge together...
type NavMenuProps =
    LoginFormStore.LoginFormState // ... state we've requested from the Redux store
    & typeof LoginFormStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps; // ... plus incoming routing parameters

class NavMenu extends React.PureComponent<NavMenuProps, any> {
    public render() {
        return (
            <header>
                <div className="challenger-nav">
                    <Link className="logo-container" to="/">
                        <img className="logo" src="logo.png" alt="Challenger" />
                        <span>Challenger</span>
                    </Link>
                    <div className="menu">
                        {
                            (!!this.props.user) &&
                                <Link to="/new">Create tournament</Link>
                        }
                        {
                            (!this.props.user) &&
                            <Link to="/signup">Sign Up</Link>
                        }
                        {
                            (!this.props.user) ?
                                (<Link to="/login">Log In</Link>) :
                                (<Link to="/" onClick={() => this.props.logOut()}>Log out</Link>)
                        }
                    </div>
                </div>  
            </header>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.login, // Selects which state properties are merged into the component's props
    LoginFormStore.actionCreators // Selects which action creators are merged into the component's props
)(NavMenu as any);
