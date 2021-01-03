import React from 'react'
import Chip from "../Chip"
import ChipContainer from "../ChipContainer"

import './style.css'

export default class Player extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            chips: props.chips,
            name: props.name,
            isScratched: props.isScratched
        }
    }

    render() {
        return (
            <div className="Player">
                <img style={{display: this.state.isScratched ? 'block' : 'none'}} src="/scratching.gif"
                     alt="Grattando la testa..." className="Cat"/>
                <ChipContainer>
                    {this.state.chips === null ? (
                        <Chip value={null}/>

                    ) : (this.state.chips.map((chip) => {
                            return (
                                <Chip value={chip}/>
                            )
                        })
                    )}
                </ChipContainer>
                <div className="Name" style={{
                    visibility: this.state.name === null ? 'hidden' : 'visible'
                }}>
                    {this.state.name}
                </div>
            </div>
        )
    }
}