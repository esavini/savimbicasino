import React from "react";
import './style.css'
import {Alert, Button} from "react-bootstrap";
import Chip from "../Chip";
import ChipContainer from "../ChipContainer";
import {HubConnectionBuilder} from "@aspnet/signalr";
import UserContext from "../UserContext";
import {withRouter} from "react-router-dom";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faUndo} from "@fortawesome/free-solid-svg-icons/faUndo";

class PlayerConsole extends React.Component {

    state = {
        money: 0
        , errors: []
        , info: []
        , availableChips: []
        , betsHistory: []
        , currentBet: {
            main: null
            , side23p1: null
            , sidePerfectPair: null
        }
        , selectedPushArea: 'main'
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
        this.undoBet = this.undoBet.bind(this)
        this.updateBetDisplay = this.updateBetDisplay.bind(this)
    }

    bet(event) {
        if (this.state.canBet !== true) {
            return
        }

        let newState = {
            ...this.state
        }

        newState.money -= event.reactState.value
        
        newState.betsHistory.push({
            type: this.state.selectedPushArea
            , value: event.reactState.value
        })

        this.setState(newState, () => {
            this.updateBetDisplay(() => {
                this.connection.send('Bet', this.state.currentBet, false)
            })
        })
    }

    updateBetDisplay(cb) {

        let newState = {
            ...this.state
        }

        let mainBet = newState.betsHistory.filter(val => val.type === 'main')

        if (mainBet.length === 0) {
            newState.currentBet.main = null
        } else {
            newState.currentBet.main = mainBet.reduce((item1, item2) => item1 + item2.value, 0)
        }

        this.setState(newState, cb)
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

        this.connection.on('BetAccepted', data => {
          /*  let newState = {
                ...this.state
                , ...data
                , availableChips: this.money2Chips(data.money)
            }

            this.setState(newState) */
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

    undoBet() {
        let newState = {
            ...this.state
        }

        let bet = newState.betsHistory.pop()
        
        newState.money += bet.value
        
        this.setState(newState, () => {
            this.updateBetDisplay()
        })
    }
    
    render() {
        console.log(this.state.money)
        
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

                <div className="CurrentBetContainer">
                    <ChipContainer>
                        <div>
                            <p>23 + 1<br/>&nbsp;</p>
                            <Chip value={this.state.currentBet.side23p1}/>
                        </div>
                        <div>
                            <p>&nbsp;<br/>&nbsp;</p>
                            <Chip value={this.state.currentBet.main}/>
                        </div>
                        <div>
                            <p>Perfect Pair</p>
                            <Chip value={this.state.currentBet.sidePerfectPair}/>
                        </div>
                    </ChipContainer>
                </div>

                <Button disabled={!this.state.canDivide} className="PlayerButton" onClick={this.divide}>Dividi</Button>
                <Button disabled={!this.state.canDouble} variant="warning" className="PlayerButton"
                        onClick={this.double}>Raddoppia</Button>
                <div className={'ChipsBetButtons ' + (this.state.canBet ? 'ChipsVisible' : 'ChipsInvisible')}>
                    <ChipContainer variant="vertical">
                        {
                            this.state.availableChips.map((value, index) => {
                                return <Chip key={index} value={value} onClick={this.bet}/>
                            })
                        }
                        {
                            this.state.betsHistory.length > 0 && (
                                <Chip value={<FontAwesomeIcon icon={faUndo}/>} onClick={this.undoBet}/>
                            )
                        }
                    </ChipContainer>
                </div>
            </div>
    );
    }
    }

    PlayerConsole.contextType = UserContext

    export default withRouter(PlayerConsole)
