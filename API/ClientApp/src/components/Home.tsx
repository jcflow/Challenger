import * as React from 'react';
import { connect } from 'react-redux';
import * as HomeStore from "../store/Home";
import {RouteComponentProps} from "react-router";
import TileContainer from "./common/TileContainer";
import {ApplicationState} from "../store";

// At runtime, Redux will merge together...
type HomeProps =
    HomeStore.HomeState // ... state we've requested from the Redux store
    & typeof HomeStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps; // ... plus incoming routing parameters

type HomeState = any;

class Home extends React.Component<HomeProps, HomeState> {
    constructor(props: any) {
        super(props);
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    public render() {
        const elements = this.props.categories.map(category => ({ id: category.id, text: category.name, imageUrl: "/resources/" + category.id + ".jpg" }));
        return (
            <React.Fragment>
                <div className="title">
                    <h1>Games</h1>
                </div>
                <TileContainer baseRoute="/category/" elements={elements} />
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        this.props.requestCategories();
    }
}

export default connect(
    (state: ApplicationState) => state.home, // Selects which state properties are merged into the component's props
    HomeStore.actionCreators // Selects which action creators are merged into the component's props
)(Home as any);