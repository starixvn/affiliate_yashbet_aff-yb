import React from 'react';

import { Input } from 'reactstrap';
import { commissionValueList, format } from '../../../modules/constant';

export default class TierSettingItem extends React.Component {
    constructor(props) {
        super(props);
    }

    handleChange = (event) => {
        const target = event.target;
        const value = Number(target.value.replace(/,/g, ''));
        const name = target.name;

        const { data, onChange } = this.props;

        const newItem = { [name]: value || 0 };

        onChange && onChange(data.TierId, newItem);
    }

    render() {
        const { data, handleClick, btnText } = this.props;
        const { TierId, MinAmount, MinAp, Commission } = data;

        return (
            <tr>
                <td className="text-center">{TierId}</td>
                <td>
                    <Input type="text" name="MinAmount" id={"MinAmount" + TierId}
                        value={format(MinAmount)} onChange={this.handleChange} />
                </td>
                <td>
                    <Input type="text" name="MinAp" id={"MinAp" + TierId} value={format(MinAp)} onChange={this.handleChange} />
                </td>
                <td>
                    <select className="form-control" name="Commission"
                        onChange={this.handleChange} value={format(Commission || 0, 3)}>
                        {
                            Object.keys(commissionValueList).map((key, i) => {
                                return (<option key={`comm-${i}`} value={key}>{commissionValueList[key]}</option>);
                            })
                        }
                    </select>
                </td>
                <td className="text-center">
                    <button type="button" className="btn btn-action" onClick={handleClick}>{btnText}</button>
                </td>
            </tr>
        )
    }
}