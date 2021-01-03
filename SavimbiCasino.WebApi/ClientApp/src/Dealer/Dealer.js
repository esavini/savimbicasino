import Player from "../Player";
import React from "react";

import './style.css'

export default class Dealer extends React.Component {

    render() {
        return (
            <div className="Dealer">
                <header className="DealerHeader">
                    ♥♠♦♣ Savimbi Casinò S.p.A. ♣♦♠♥
                </header>
                <div className="Container">
                    <div className="Players">
                        <Player name={null}
                                chips={null}/>
                        <Player name={'Ciao'}
                                chips={[2]}/>
                        <Player name={'Marti'}
                                chips={[50]}
                                isScratched={true}/>
                        <Player name={'Ciao'}
                                chips={[5]}/>
                        <Player name={'Ciao'}
                                chips={[10]}/>
                        <Player name={'Ciao'}
                                chips={[100]}/>
                        <Player name={'Ciao'}
                                chips={[20, 20]}/>
                    </div>
                </div>
            </div>
        );
    }
}