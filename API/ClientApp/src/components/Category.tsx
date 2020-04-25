import * as React from 'react';
import { connect } from 'react-redux';
import * as CategoryStore from "../store/Category";
import {RouteComponentProps} from "react-router";
import TileContainer from "./common/TileContainer";
import { ApplicationState } from "../store";
import WebSocket from '../websocket.js';

// At runtime, Redux will merge together...
type CategoryProps =
    CategoryStore.CategoryState // ... state we've requested from the Redux store
    & typeof CategoryStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ id: string }>; // ... plus incoming routing parameters

type CategoryState = any;

class Category extends React.Component<CategoryProps, CategoryState> {
    // This method is called when the component is first added to the document
    public componentDidMount() {
        const self = this;
        self.ensureDataFetched();
        WebSocket.onmessage = (event) => {
            if (event.data === "UPDATE") {
                self.ensureDataFetched();
            }
        };
    }

    public render() {
        const categoryId = parseInt(this.props.match.params.id, 10) || 0;
        const elements = this.props.tournaments.map(tournament => ({ id: tournament.id, text: tournament.name, imageUrl: "/resources/" + categoryId + ".jpg" }));
        return (
            <React.Fragment>
                <div className="title">
                    <h1>Tournaments</h1>
                </div>
                <TileContainer baseRoute="/tournament/" elements={elements} />
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        const id = parseInt(this.props.match.params.id, 10) || 0;
        this.props.requestTournamentsByCategoryId(id);
    }
}

export default connect(
    (state: ApplicationState) => state.category, // Selects which state properties are merged into the component's props
    CategoryStore.actionCreators // Selects which action creators are merged into the component's props
)(Category as any);
