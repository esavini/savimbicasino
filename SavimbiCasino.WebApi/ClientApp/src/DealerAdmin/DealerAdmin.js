import React from "react";

import './style.css'
import {Button, Table} from "react-bootstrap";
import {withRouter} from "react-router-dom"
import {HubConnectionBuilder} from "@aspnet/signalr";

class DealerAdmin extends React.Component {

    state = {
        status: -1
        , players: []
    }

    constructor(params) {
        super(params);

        this.connection = new HubConnectionBuilder()
            .withUrl(window.location.origin + "/v1/hubs/room")
            .build()

        this.changeStatus = this.changeStatus.bind(this)
        this.charge = this.charge.bind(this)
        this.kick = this.kick.bind(this)
        this.doubleWin = this.doubleWin.bind(this)
        this.win = this.win.bind(this)
        this.blackjack = this.blackjack.bind(this)
        this.push = this.push.bind(this)
    }

    changeStatus(event) {
        this.connection.send("ChangeGameStatus", parseInt(event.target.attributes.value.value))
    }

    win(event) {
        this.connection.send("Win", event.target.attributes.value.value)
    }

    push(event) {
        this.connection.send("Push", event.target.attributes.value.value)
    }

    doubleWin(event) {
        this.connection.send("WinDouble", event.target.attributes.value.value)
    }

    kick(event) {
        this.connection.send("Kick", event.target.attributes.value.value)
    }

    blackjack(event) {
        this.connection.send("Blackjack", event.target.attributes.value.value)
    }

    charge(event) {
        this.connection.send("Charge", event.target.attributes.value.value)
    }

    componentDidMount() {
        let id = this.props.match.params.roomId

        this.connection.start()
            .then(conn => {
                this.connection.send("DealerAdminJoin", id)

                this.connection.on('UpdateDealerAdmin', room => {
                    console.log(room)

                    let newState = {
                        ...this.state
                        , ...room
                    }

                    this.setState(() => newState)
                })

            })
            .catch(err => console.error(err))
    }

    render() {
        return (
            <div className="PlayerConsole">
                <h1>Pannello di controllo della partita</h1>
                <div>
                    <Button variant="success" value={1} disabled={this.state.status !== 0} onClick={this.changeStatus}>Abilita
                        Puntate</Button>
                    <Button variant="success" value={2} disabled={this.state.status !== 1} onClick={this.changeStatus}>Disabilita
                        Puntate</Button>
                    <Button variant="success" value={3} disabled={this.state.status !== 2} onClick={this.changeStatus}>Abilita
                        Gioco</Button>
                    <Button variant="danger" value={0} disabled={this.state.status !== 3} onClick={this.changeStatus}>Termina
                        Turno</Button>
                    <p>Se termini il turno e non hai segnato la vincita, i giocatori perderanno la puntata.</p>
                </div>

                <Table striped bordered hover>
                    <thead>
                    <tr>
                        <th>#</th>
                        <th>Nome</th>
                        <th>Note</th>
                        <th>Azioni</th>
                        <th>Ricarica</th>
                    </tr>
                    </thead>
                    <tbody>

                    {
                        this.state.players.map((player, index) => {
                                return (
                                    <tr>
                                        <td>{index + 1}</td>
                                        <td>{player.username}</td>
                                        <td>
                                            <p>Puntata: € {player.bet}</p>
                                            <p>Raddoppio: {
                                                player.isDouble ? (<span style={{color: "red"}}>Sì</span>) : (
                                                    <span>No</span>)
                                            }</p>
                                            <p>Divisa: {
                                                player.isDivided ? (<span style={{color: "red"}}>Sì</span>) : (
                                                    <span>No</span>)
                                            }</p>
                                        </td>
                                        <td>
                                            <div>
                                                <Button variant="success" value={player.userId}
                                                        onClick={this.win}>Win</Button>
                                                <Button variant="info" disabled={!player.isDivided} value={player.userId}
                                                        onClick={this.doubleWin}>Win Doppia</Button>
                                                <Button variant="dark"
                                                        disabled={player.isDivided || player.isDouble} value={player.userId}
                                                        onClick={this.blackjack}>BlackJack</Button>
                                                <Button variant="primary" value={player.userId}
                                                        onClick={this.win}>Push</Button>
                                            </div>

                                            <Button variant="warning" style={{marginTop: '20px'}} value={player.userId}
                                                    onClick={this.kick}>Espelli</Button>
                                        </td>
                                        <td>
                                            <Button value={player.userId} onClick={this.charge}>Ricarica 25</Button>
                                        </td>
                                    </tr>
                                )
                            }
                        )
                    }
                    </tbody>
                </Table>
            </div>
        )
    }
}

export default withRouter(DealerAdmin)