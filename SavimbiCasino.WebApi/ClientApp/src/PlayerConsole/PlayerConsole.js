import React from "react";

import './style.css'
import {Button} from "react-bootstrap";
import Chip from "../Chip";
import ChipContainer from "../ChipContainer";

export default class PlayerConsole extends React.Component {

    constructor(params) {
        super(params);
        
        this.state = {
            money: 10
        }
    }
    
    bet(event) {
        console.log(event.reactState.value)
    }
    
    render() {
        return (
            <div className="PlayerConsole">
                <p>Credito: € {this.state.money}</p>
                <Button variant="success">Carta</Button>
                <Button variant="danger">Stop</Button>
                <Button>Dividi</Button>
                <Button variant="light">Raddoppia</Button>
                <ChipContainer style={{display:'block'}}>
                    <Chip value={1} onClick={this.bet}/>
                    <Chip value={5} onClick={this.bet}/>
                    <Chip value={10}/>
                    <Chip value={20}/>
                </ChipContainer>
            </div>
        );
    }
}