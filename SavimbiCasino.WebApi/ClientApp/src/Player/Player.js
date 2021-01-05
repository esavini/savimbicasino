import React from 'react'
import Chip from "../Chip"
import ChipContainer from "../ChipContainer"

import './style.css'

export default class Player extends React.Component {

    state = {
        chips: null,
        name: null,
        isScratched: false
    }

    constructor(props) {
        super(props)

        this.state = {
            chips: props.chips,
            name: props.name,
            isScratched: props.isScratched,
            hash: props.hash
        }
    }

    render() {
        return (
            <div className="Player">
                <img style={{display: this.props.isScratched ? 'block' : 'none'}} src="/scratching.gif"
                     alt="Grattando la testa..." className="Cat"/>
                <ChipContainer>
                    {this.props.chips === null ? (
                        <Chip value={null}/>

                    ) : (this.props.chips.map((chip, index) => {
                            return (
                                <Chip key={index} value={chip}/>
                            )
                        })
                    )}
                </ChipContainer>
                <div className="Name" style={{
                    visibility: this.props.name === null ? 'hidden' : 'visible'
                }}>
                    {this.props.name}
                </div>
            </div>
        )
    }
}