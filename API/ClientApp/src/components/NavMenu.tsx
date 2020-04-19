import * as React from 'react';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public render() {
        return (
            <header>
                <div className="challenger-nav">
                    <Link className="logo-container" to="/">
                        <img className="logo" src="logo.png" alt="Challenger" />
                        <span>Challenger</span>
                    </Link>
                    <div>
                        <Link to="/new">Create tournament</Link>
                    </div>
                </div>
            </header>
        );
    }
}
