import * as React from "react";

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

class TileContainer extends React.PureComponent<TileContainerProps, TileContainerState> {
    public render() {
        const delay = 200;
        return (
            <div className="tile-container">
                {
                    this.props.elements.map((element, index) => {
                        const style = {
                            animationDelay: `${index * delay}ms`
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
export default TileContainer;
