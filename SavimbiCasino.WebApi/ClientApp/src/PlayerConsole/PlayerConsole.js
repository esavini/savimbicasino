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
        ,errors: []
        ,info: []
    }
    
    constructor(params) {
        super(params);

        this.connection = new HubConnectionBuilder()
            .withUrl(window.location.origin + "/v1/hubs/room")
            .build()
    }

    bet(event) {
        console.log(event.reactState.value)
    }

    componentDidMount() {
        let id = this.props.computedMatch.params.roomId

        this.connection.start()
            .then(conn => {
                
                this.connection.send("Login", this.context.user.token, id)

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

            })
            .catch(err => {
                let newState = {
                    ...this.state
                }
                
                newState.errors.push("Impossibile stabilire una connessione con la stanza di gioco. " + err)
                
                this.setState(newState)
            })
    }

    render() {
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
                <Button variant="success">Carta</Button>
                <Button variant="danger">Stop</Button>
                <Button>Dividi</Button>
                <Button variant="warning">Raddoppia</Button>
                <ChipContainer style={{display: 'block'}}>
                    <Chip value={1} onClick={this.bet}/>
                    <Chip value={5} onClick={this.bet}/>
                    <Chip value={10}/>
                    <Chip value={20}/>
                </ChipContainer>
            </div>
        );
    }
}

PlayerConsole.contextType = UserContext

export default withRouter(PlayerConsole)
