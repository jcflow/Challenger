// @ts-nocheck
import _ from 'lodash';
import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as TournamentStore from '../store/Tournament';
import {Bracket, Score} from "../store/Tournament";

// At runtime, Redux will merge together...
type TournamentProps =
    TournamentStore.TournamentState // ... state we've requested from the Redux store
  & typeof TournamentStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ id: string }>; // ... plus incoming routing parameters


class Tournament extends React.PureComponent<TournamentProps> {
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
    const { name, brackets } = this.props.tournament;
    const subtitle = "Ice hockey at the 1998 Winter Olympics – Men's tournament";

    const sortedBrackets = _.orderBy(brackets, ['level']);
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
                <table className="tournament-bracket__table">
                  <tbody className="tournament-bracket__content">
                  {
                    bracket.scores.map(score => this.renderScore(score, isWinner(score)))
                  }
                  </tbody>
                </table>
              </div>
            </li>
    );
  }

  private renderScore(score: Score, isWinner: boolean) {
    return (
        <tr key={score.id} className={"tournament-bracket__team" + (isWinner ? " tournament-bracket__team--winner" : "") }>
          <td className="tournament-bracket__country">
            <div className="tournament-bracket__code" title="Finland">{score.team.name}</div>
          </td>
          <td className="tournament-bracket__score">
            <span className="tournament-bracket__number">{score.value}</span>
          </td>
        </tr>
    );
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
