describe("Layout", function () {
    describe('initialize', function () {
        it('initial functions should be called', function () {
            spyOn($.fn, 'carousel');
            spyOn(Layout, 'registerAjaxHandlerError');
            spyOn(Layout, 'initEvent');
            spyOn(Layout, 'renderHotNews');

            Layout.initialize();

            expect($.fn.carousel).toHaveBeenCalled();
            expect(Layout.registerAjaxHandlerError).toHaveBeenCalled();
            expect(Layout.initEvent).toHaveBeenCalled();
            expect(Layout.renderHotNews).toHaveBeenCalled();
        });
    })
});