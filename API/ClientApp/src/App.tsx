import * as React from 'react';
import { Route, Redirect, Switch } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Tournament from './components/Tournament';
import TournamentForm from "./components/TournamentForm";
import Category from "./components/Category";
import LoginForm from "./components/LoginForm";
import SignUpForm from './components/SignUpForm';
import NotFound from './components/404';

import './reset.css';
import './custom.css';
import './tournament.css';

export default () => (
    <Layout>
        <Switch>
            <Route exact path='/' component={Home} />
            <Route path='/signup' component={SignUpForm} />
            <Route path='/login' component={LoginForm} />
            <Route path='/new' component={TournamentForm} />
            <Route path='/category/:id?' component={Category} />
            <Route path='/tournament/:id?' component={Tournament} />
            <Route path='/404' component={NotFound} />
            <Redirect to='/404' />
        </Switch>
    </Layout>
);
