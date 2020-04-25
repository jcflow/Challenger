import * as React from 'react';

// At runtime, Redux will merge together...
type LoaderProps = {
    active: boolean
};

class Loader extends React.PureComponent<LoaderProps, any> {
    public render() {
        return (
            <React.Fragment>
                {
                    (this.props.active) &&
                    <div className="loader">
                        <div className="lds-grid"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>
                    </div>
                }
            </React.Fragment>
        );
    }
}

export default (Loader as any);
