import React from "react";
import {HubConnectionBuilder} from "@aspnet/signalr";
import {withRouter} from "react-router-dom";

import Player from "../Player";

import './style.css'


class Dealer extends React.Component {

    state = {
        players: this.fillPlayers([])
    }

    constructor(props) {
        super(props);

        this.connection = new HubConnectionBuilder()
            .withUrl(window.location.origin + "/v1/hubs/room")
            .build()
    }

    fillPlayers(players) {
        while (players.length < 7) {
            players.push({
                id: this.generateGuid()
                , name: null
                , chips: null
            })
        }

        return players
    }

    generateGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            let r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8)
            return v.toString(16)
        })
    }

    componentDidMount() {
        let id = this.props.match.params.roomId

        this.connection.start()
            .then(conn => {
                this.connection.send("DealerJoin", id)

                this.connection.on('UpdateRoom', room => {
                    console.log(room)

                    let newState = {
                        ...this.state
                        , players: this.fillPlayers(room.players ?? [])
                    }

                    this.setState(() => newState)
                    
                    setTimeout(() => this.connection.send("RefreshRoom"), 1000)
                })

            })
            .catch(err => console.error(err))
    }

    render() {
        return (
            <div className="Dealer">
                <header className="DealerHeader">
                    ♥♠♦♣ Savimbi Casinò S.p.A. ♣♦♠♥
                </header>
                <div className="Container">
                    <div className="Players">
                        {
                            this.state.players.map((player) => {
                                return <Player key={player.id} name={player.name} chips={player.chips} isScratched={player.isScratched}/>
                            })
                        }
                    </div>
                </div>
            </div>
        );
    }
}

export default withRouter(Dealer)