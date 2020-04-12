// @ts-nocheck
import _ from 'lodash';
import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as TournamentStore from '../store/Tournament';
import {Bracket, Score} from "../store/Tournament";
import { Badge } from 'reactstrap';

// At runtime, Redux will merge together...
type TournamentProps =
    TournamentStore.TournamentState // ... state we've requested from the Redux store
  & typeof TournamentStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ id: string }>; // ... plus incoming routing parameters

type TournamentState = any;

class Tournament extends React.PureComponent<TournamentProps, TournamentState> {

    private scoreInputRef = React.createRef<HTMLInputElement>();

    constructor(props: any) {
        super(props);
        this.state = {
            editingScore: false,
            scoreId: 0
        };
    }

  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  public render() {
    return (
      <React.Fragment>
        <h1>{this.props.tournament && this.props.tournament.name }</h1>
        <div>{this.props.tournament && this.renderTournament()}</div>
      </React.Fragment>
    );
  }

  private renderTournament() {
    // @ts-ignore
    const { brackets } = this.props.tournament;
      let sortedBrackets = _.orderBy(brackets, ['level']);
      const minimumLevel = sortedBrackets[0].level;
      //if (minimumLevel > 0) {
      //    for (let level = minimumLevel - 1; level >= 0; level--) {
      //        for (let bracketNumber = 0; bracketNumber < Math.pow(2, level); bracketNumber++) {
      //            const scores = [];
      //            for (let scoreNumber = 0; scoreNumber < 2; scoreNumber++) {
      //                scores.push({
      //                    id: `${level},${bracketNumber},${scoreNumber}`,
      //                    value: 0,
      //                    team: {
      //                        id: `${level},${bracketNumber},${scoreNumber},${scoreNumber}`,
      //                        name: "?"
      //                    }
      //                });
      //            }
      //            sortedBrackets.push({
      //                id: `${level},${bracketNumber}`,
      //                level: level,
      //                finished: false,
      //                scores: scores
      //            });
      //        }
      //    }
      //    sortedBrackets = _.orderBy(sortedBrackets, ['level']);
      //}
    const rounds = (Object.values(_.groupBy(sortedBrackets, x => x.level)).reverse()) as Array<Bracket[]>;
    return (
        <div className="container">
          <div className="tournament-bracket tournament-bracket--rounded">
            {
              rounds.map((round, index) => this.renderRound(round, index))
            }
          </div>
        </div>
    );
  }

  private renderRound(round: Bracket[], index: number) {
    return (
        <div key={index} className="tournament-bracket__round">
          <ul className="tournament-bracket__list">
            {
              round.map(bracket => this.renderBracket(bracket))
            }
          </ul>
        </div>
    );
  }

  private renderBracket(bracket: Bracket) {
    const isWinner = (score: Score) => {
      const winner = _.orderBy(bracket.scores, ['value'], ['desc'])[0];
      return score.id === winner.id;
    };

    return (
        <li key={bracket.id} className="tournament-bracket__item">
              <div className="tournament-bracket__match">
                  {
                      !bracket.finished &&
                      <Badge className="closeBadge" color="success" onClick={() => this.props.closeBracket(bracket.id)}>Close</Badge>
                  }
                <table className="tournament-bracket__table">
                  <tbody className="tournament-bracket__content">
                  {
                    bracket.scores.map(score => this.renderScore(score, bracket.finished, isWinner(score)))
                  }
                  </tbody>
                </table>
              </div>
            </li>
    );
  }

  private renderScore(score: Score, finished: boolean, isWinner: boolean) {
        let scoreComponent = null;
        if (finished) {
            scoreComponent = <span className="tournament-bracket__number">{score.value}</span>;
        } else if (this.state.scoreId === score.id && this.state.editingScore) {
            scoreComponent = <input type="number"
                                    className="tournament-bracket__number"
                                    autoFocus
                                    ref={this.scoreInputRef}
                                    defaultValue={score.value}
                                    onBlur={() => this.handleScoreUpdate()}/>;
        } else {
            scoreComponent = <span className="tournament-bracket__number"
                                   onClick={() => this.setState({ scoreId: score.id, editingScore: true })}>{score.value}</span>;
        }

    return (
        <tr key={score.id} className={"tournament-bracket__team" + (isWinner && finished ? " tournament-bracket__team--winner" : "") }>
          <td className="tournament-bracket__country">
            <div className="tournament-bracket__code">{score.team.name}</div>
          </td>
          <td className="tournament-bracket__score">
              { scoreComponent }
          </td>
        </tr>
    );
  }

  private handleScoreUpdate() {
      const scoreValue = Number(this.scoreInputRef.current!.value);
      const scoreId = this.state.scoreId;
      this.props.updateScore(scoreId, scoreValue);
      this.setState({ scoreId: 0, editingScore: false });
  }

  private ensureDataFetched() {
    const id = parseInt(this.props.match.params.id, 10) || 0;
    this.props.requestTournament(id);
  }
}

export default connect(
  (state: ApplicationState) => state.tournament, // Selects which state properties are merged into the component's props
    TournamentStore.actionCreators // Selects which action creators are merged into the component's props
)(Tournament as any);
