import React from 'react'
import './style.css'

export default class ChipContainer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            children: props.children,
        }
    }

    render() {
        return (
            <div className="Chips">
                {this.state.children}
            </div>
        )
    }
}