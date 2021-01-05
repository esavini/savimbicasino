import React from 'react'
import './style.css'

export default class ChipContainer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isVertical: this.props.variant === 'vertical'
        }

        this.applyClassNames = this.applyClassNames.bind(this)
    }

    applyClassNames() {
        let classNames = ['Chips']

        if(this.state.isVertical) {
            classNames.push('ChipsVertical')
        }
        
        return classNames.join(' ')
    }

    render() {
        return (
            <div className={this.applyClassNames()}>
                {this.props.children}
            </div>
        )
    }
}