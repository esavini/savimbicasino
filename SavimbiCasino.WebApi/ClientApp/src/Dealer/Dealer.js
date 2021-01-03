import Player from "../Player";
import React, { useState, useEffect } from "react";

import './style.css'
import {HubConnectionBuilder} from "@aspnet/signalr";

export default class Dealer extends React.Component {
    
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
                ,name: null
                ,chips: null
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
        this.connection.start()
            .then(conn => {
                this.connection.send("DealerJoin", "38022e9f-40dd-4123-969a-55a3f237d2a9")

                this.connection.on('UpdateRoom', room => {
                    console.log(room)

                    let newState = {
                        ...this.state
                        , players: this.fillPlayers(room.players ?? [])
                    }
                    
                    this.setState(() => newState)
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
                                return <Player key={player.id} name={player.name} chips={player.chips}/>
                            })
                        }
                    </div>
                </div>
            </div>
        );
    }
}