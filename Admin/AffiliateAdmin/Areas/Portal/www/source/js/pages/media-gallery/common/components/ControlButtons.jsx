import React from 'react';
import { Button } from 'reactstrap';

import SelectAll from '../../../common/components/SelectAll';
import ButtonDeleteItems from '../../../common/components/ButtonDeleteItems';
import { defaultInsertId, globalResource } from '../libs/constants';

export const ControlButtons = (props) => {

    function _handleAddItem() {
        props.setSelectedItemId(defaultInsertId);
        props.setShouldResetScrollPosition(false);
    }

    const hasData = props.gridData && props.gridData.length;

    return props.gridData && (
        <React.Fragment>
            {
                hasData ? (
                    <React.Fragment>
                        <div className="my-2">
                            <ButtonDeleteItems
                                checkedItems={props.checkedItems}
                                toggleLoader={props.toggleLoader}
                                deleteItems={props.deleteItems}
                                fetchData={() => props.fetchData(props.param)}
                                messages={props.messages}
                                hasIcon={true}
                                isOpenNotification={true}
                            />
                            <Button outline type="button" className="mx-2" onClick={_handleAddItem}>{globalResource.Add} <span className="oi oi-plus ml-2"></span></Button>
                        </div>
                        <SelectAll
                            checkedList={props.checkedItems}
                            setCheckedList={props.setCheckedItems}
                            setShouldResetScrollPosition={props.setShouldResetScrollPosition}
                        />
                    </React.Fragment>
                ) : (
                        <div className="my-2">
                            <Button outline type="button" onClick={_handleAddItem}>{globalResource.Add} <span className="oi oi-plus ml-2"></span></Button>
                        </div>
                    )
            }
        </React.Fragment>
    );
}

export default (React.memo(ControlButtons));