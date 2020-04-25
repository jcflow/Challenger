import * as React from 'react';

class NotFound extends React.PureComponent<any, any> {
    public render() {
        return (
            <React.Fragment>
                <div className="title">
                    <h1>404</h1>
                </div>
                <div className="centered-container">
                    <span role="img">💩</span>
                </div>
            </React.Fragment>
        );
    }
}

export default (NotFound as any);
