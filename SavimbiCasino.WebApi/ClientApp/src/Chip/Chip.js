import React from 'react'
import './style.css'

export default class Chip extends React.Component {

    constructor(props) {
        super(props)

        this.state = {
            value: this.props.value
            , color: this.getColor()
            , props: this.props
        }

        this.onClick = this.onClick.bind(this)
        this.render = this.render.bind(this)
    }

    componentDidUpdate(prevProps, prevState, snapshot) {

        if (prevProps.value === this.props.value) {
            return
        }

        let newState = {
            ...this.state
            , color: this.getColor()
        }

        this.setState(newState)
    }

    getColor() {
        let color

        if (this.props.value === null || typeof this.props.value === "object")
            color = 'rgba(10, 10, 10, .3)'

        else if (this.props.value < 5) // 1
            color = '#000'

        else if (this.props.value < 10) // 5
            color = '#0c9802'

        else if (this.props.value < 20) // 10
            color = '#023998'

        else if (this.props.value < 50) // 20
            color = '#c81e83'

        else if (this.props.value < 100) // 50
            color = '#c8b71e'

        else // >= 100
            color = '#c8241e'

        return color
    }

    onClick(event) {
        event.reactState = this.state

        if (this.props.onClick !== undefined)
            this.props.onClick(event)
    }

    render() {
        return (
            <div className="ChipContainer">
                <div className="ChipBefore" style={{background: this.state.color}}/>
                <div className="Chip">
                    {
                        (this.props.value !== null && typeof this.props.value === "object") ? (
                            <this.props.value.type {...this.props.value.props}/>
                        ) : (
                            this.props.value
                        )
                    }
                </div>
                <div className="ChipContainerEventArea" onClick={this.onClick}/>
            </div>
        )
    }
}