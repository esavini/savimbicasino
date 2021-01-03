import React from "react";

import './style.css'
import {Button, Table} from "react-bootstrap";

export default class DealerAdmin extends React.Component {

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

                {/*globali*/}
                <Button variant="success">Abilita Puntate</Button>
                <Button variant="success">Disabilita Puntate</Button>
                <Button variant="danger">Termina Turno</Button>


                {/*da copiare per ogni player*/}


                <Table striped bordered hover>
                    <thead>
                    <tr>
                        <th>Player1</th>
                        <th>Player2</th>
                        <th>Player3</th>
                        <th>Player4</th>
                        <th>Player5</th>
                        <th>Player6</th>
                        <th>Player7</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td><Button variant="success">Win</Button>
                            <br/>
                            <Button variant="info">WinDoppia</Button>
                            <br/>
                            <Button variant="dark">BlackJack</Button>
                            <br/>
                            <Button variant="warning">Espelli</Button>
                        </td>
                        <td><Button variant="success">Win</Button>
                            <br/>
                            <Button variant="warning">Espelli</Button>
                        </td>
                        <td><Button variant="success">Win</Button>
                            <br/>
                            <Button variant="warning">Espelli</Button>
                        </td>
                        <td><Button variant="success">Win</Button>
                            <br/>
                            <Button variant="warning">Espelli</Button>
                        </td>
                        <td><Button variant="success">Win</Button>
                            <br/>
                            <Button variant="warning">Espelli</Button>
                        </td>
                            
                    </tr>


                    </tbody>
                </Table>
            </div>
        );
    }
}