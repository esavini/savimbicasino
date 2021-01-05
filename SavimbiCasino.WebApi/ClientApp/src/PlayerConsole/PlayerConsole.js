import React from "react";
import './style.css'
import {Alert, Button} from "react-bootstrap";
import Chip from "../Chip";
import ChipContainer from "../ChipContainer";
import {HubConnectionBuilder} from "@aspnet/signalr";
import UserContext from "../UserContext";
import {withRouter} from "react-router-dom";

class PlayerConsole extends React.Component {

    state = {
        money: 0
        , errors: []
        , info: []
        , availableChips: []
        , canDivide: false
        , canDouble: false
        , canBet: false
    }

    constructor(params) {
        super(params);

        this.connection = new HubConnectionBuilder()
            .withUrl(window.location.origin + "/v1/hubs/room")
            .build()
        
        this.bet = this.bet.bind(this)
        this.divide = this.divide.bind(this)
        this.double = this.double.bind(this)
    }

    bet(event) {
        if(this.state.canBet !== true) {
            return
        }
        
        this.connection.send("Bet", event.reactState.value)
    }
    
    divide() {
        this.connection.send("Divide")
    }
    
    double() {
        this.connection.send("Double")
    }

    componentDidMount() {
        let id = this.props.computedMatch.params.roomId

        this.connection.start()
            .then(conn => {

                this.connection.send("Login", this.context.user.token, id)

            })
            .catch(err => {
                let newState = {
                    ...this.state
                }

                newState.errors.push("Impossibile stabilire una connessione con la stanza di gioco. " + err)

                this.setState(newState)
            })

        this.connection.on('RoomClosed', room => {
            let newState = {
                ...this.state
            }

            newState.info.push("Stanza di gioco chiusa.")

            this.setState(newState)

            this.connection.stop().catch(err => {
                let newState = {
                    ...this.state
                }

                newState.errors.push("Impossibile chiudere correttamente la connessione con la stanza di gioco. " + err)

                this.setState(newState)
            })
        })

        this.connection.on('UpdatePlayer', data => {
            let newState = {
                ...this.state
                , ...data
                , availableChips: this.money2Chips(data.money)
            }

            this.setState(newState)
        })
    }

    money2Chips(money) {
        let chips = []

        const availableChips = [1, 5, 10, 20, 50, 100]

        for (let i = 0; i < availableChips.length; i++) {
            if (money >= availableChips[i]) {
                chips.push(availableChips[i])
            }
        }

        return chips
    }

    render() {
        const chipsVisibility = {
            display: this.state.canBet ? 'block !important' : 'none !important'
        }
        
        return (
            <div className="PlayerConsole">
                {
                    this.state.errors.map((text, index) => {
                        return <Alert key={index} variant="danger">
                            {text}
                        </Alert>
                    })
                }

                {
                    this.state.info.map((text, index) => {
                        return <Alert key={index} variant="info">
                            {text}
                        </Alert>
                    })
                }

                <p>Credito: € {this.state.money}</p>
                <Button disabled={!this.state.canDivide} className="PlayerButton" onClick={this.divide}>Dividi</Button>
                <Button disabled={!this.state.canDouble}  variant="warning" className="PlayerButton" onClick={this.double}>Raddoppia</Button>
                <ChipContainer style={chipsVisibility} variant="vertical">
                    {
                        this.state.availableChips.map((value, index) => {
                            return <Chip key={index} value={value} onClick={this.bet}/>
                        })
                    }
                </ChipContainer>
            </div>
        );
    }
}

PlayerConsole.contextType = UserContext

export default withRouter(PlayerConsole)
