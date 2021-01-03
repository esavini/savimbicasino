import React from 'react'
import './style.css'

export default class Chip extends React.Component {
    constructor(props) {
        super(props);

        let color

        if (props.value === null)
            color = 'rgba(10, 10, 10, .3)'

        else if (props.value < 5) // 1
            color = '#000'

        else if (props.value < 10) // 5
            color = '#0c9802'

        else if (props.value < 20) // 10
            color = '#023998'

        else if (props.value < 50) // 20
            color = '#c81e83'

        else if (props.value < 100) // 50
            color = '#c8b71e'

        else // >= 100
            color = '#c8241e'

        this.state = {
            value: props.value
            , color: color
            , props: props
        }
        
        this.onClick = this.onClick.bind(this)
    }
    
    onClick(event) {
        event.reactState = this.state
        
        if(this.props.onClick !== undefined)
            this.props.onClick(event)
    }

    render() {
        return (
            <div className="ChipContainer">
                <div className="ChipBefore" style={{background: this.state.color}}/>
                <div className="Chip">
                    {this.state.value}
                </div>
                <div className="ChipContainerEventArea" onClick={this.onClick}/>
            </div>
        )
    }
}