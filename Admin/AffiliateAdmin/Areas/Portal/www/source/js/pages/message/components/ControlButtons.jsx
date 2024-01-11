import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button } from 'reactstrap';

import SelectAll from '../../common/components/SelectAll';
import ButtonDeleteItems from '../../common/components/ButtonDeleteItems';
import { resource, messages, messageInsertId, messageCurrentId } from '../libs/constants';
import { fetchData, deleteMessages, toggleLoader, setCheckedMessages, setSelectedMessageId, setShouldResetScrollPosition } from '../actions';

export const ControlButtons = (props) => {

    function _handleClickButton(selectedType) {
        props.setSelectedMessageId(selectedType);
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
                                checkedItems={props.checkedMessages}
                                toggleLoader={props.toggleLoader}
                                deleteItems={props.deleteMessages}
                                fetchData={() => props.fetchData(props.param)}
                                messages={messages}
                                hasIcon={true}
                                isOpenNotification={true}
                            />
                            <Button outline type="button" className="mx-2" onClick={() => _handleClickButton(messageInsertId)}>{resource.Add} <span className="oi oi-plus ml-2"></span></Button>
                            <Button outline type="button" onClick={() => _handleClickButton(messageCurrentId)}>{resource.CurrentMessage}</Button>
                        </div>
                        <SelectAll
                            checkedList={props.checkedMessages}
                            setCheckedList={props.setCheckedMessages}
                            setShouldResetScrollPosition={props.setShouldResetScrollPosition}
                        />
                    </React.Fragment>
                ) : (
                        <div className="my-2">
                            <Button outline type="button" onClick={() => _handleClickButton(messageInsertId)}>{resource.Add} <span className="oi oi-plus ml-2"></span></Button>
                        </div>
                    )
            }
        </React.Fragment>
    );
}


function mapStateToProps({ checkedMessages, param, gridData }) {
    return Object.assign({}, { checkedMessages, param, gridData });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ fetchData, deleteMessages, toggleLoader, setCheckedMessages, setSelectedMessageId, setShouldResetScrollPosition }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(ControlButtons));