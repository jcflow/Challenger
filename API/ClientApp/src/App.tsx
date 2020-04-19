import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Tournament from './components/Tournament';
import TournamentForm from "./components/TournamentForm";
import Category from "./components/Category";

import './reset.css';
import './custom.css';
import './tournament.css';

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/new' component={TournamentForm} />
        <Route path='/category/:id?' component={Category} />
        <Route path='/tournament/:id?' component={Tournament} />
    </Layout>
);
