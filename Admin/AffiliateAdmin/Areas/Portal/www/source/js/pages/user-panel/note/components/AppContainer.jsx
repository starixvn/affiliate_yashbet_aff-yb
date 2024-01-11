import React from 'react';

import AppLayout from '../../../../layouts/AppLayout';
import Content from './Content';
import InsertUpdateNoteForm from './InsertUpdateNoteForm';

const AppContainer = (props) => {
    return (
        <AppLayout>
            <Content />
            <InsertUpdateNoteForm />
        </AppLayout>
    );
}

export default React.memo(AppContainer);