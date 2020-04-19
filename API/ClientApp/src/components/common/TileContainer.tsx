import * as React from "react";
import {connect} from "react-redux";

// At runtime, Redux will merge together...
type TileContainerProps = {
    baseRoute: string
    elements: Element[]
};

type Element = {
    id?: number
    text?: string
    imageUrl: string
}

type TileContainerState = any;

class TileContainer extends React.Component<TileContainerProps, TileContainerState> {
    public render() {
        const delay = 125;
        return (
            <div className="tile-container">
                {
                    this.props.elements.map((element, index) => {
                        const style = {
                            "animation-delay": `${index * delay}ms`
                        } as React.CSSProperties;
                        return (<a key={element.id}
                            href={this.props.baseRoute + element.id}
                            className="tile swing-in-top-fwd"
                            style={style}>
                            <img src={element.imageUrl} alt={element.text} />
                            <div className="content">{element.text}</div>
                        </a>);
                    })
                }
            </div>
        );
    }
}
export default connect()(TileContainer);
