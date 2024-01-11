import React, { Fragment} from 'react';
import { FormGroup } from 'reactstrap';
import { commissionValueList, format } from '../../../modules/constant';

export const DeductedPercentageGroup = (props) => {
    return (
        <Fragment>
            <FormGroup className="mr-2">
                <select name="DeductedPercentage" className="form-control"
                    value={format(props.value || 0, 3)}
                    onChange={props.onChange} style={{ width: 70 }}>
                    {
                        Object.keys(commissionValueList).map((key, i) => {
                            return (<option key={`deducted-percentage-${i}`} value={key}>{commissionValueList[key]}</option>);
                        })
                    }
                </select>
            </FormGroup>
            <div className="x-group">
                <label className="ml-3 x-content">{props.label}</label>
            </div>
        </Fragment>
    )
}

export default React.memo(DeductedPercentageGroup);