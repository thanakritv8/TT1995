@Code
    ViewData("Title") = "dashboard"
End Code

<style>
    .button-container {
    text-align: center;
    height: 40px;
    position: absolute;
    top: 7px;
    right: 0px;


}
.wizard .content {
    min-height: 100px;
}
.wizard .content > .body {
    width: 100%;
    height: auto;
    padding: 15px;
    position: relative;
}
</style>

    <div class="content" >
        <h1>Dashboard</h1>

        <script>
            $(function () {
                $("#wizard").steps({
                    headerTag: "h2",
                    bodyTag: "section",
                    //transitionEffect: "none",
                    enableFinishButton: false,
                    enablePagination: false,
                    enableAllSteps: true,
                    titleTemplate: "#title#",
                    //cssClass: "tabcontrol"
                });
            });
        </script>

        <div id="wizard">

            <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="pills-fleet-tabien-tab" data-toggle="pill" href="#pills-fleet-tabien" role="tab" aria-controls="pills-home" aria-selected="true">Fleet Tabien</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-license-tab" data-toggle="pill" href="#pills-license" role="tab" aria-controls="pills-profile" aria-selected="false">License</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-driver-tab" data-toggle="pill" href="#pills-driver" role="tab" aria-controls="pills-contact" aria-selected="false">Driver</a>
                </li>
                <li class="nav-item">
                    <div class="">
                        <input class="form-control form-control" id="year" value="" autocomplete="off">
                    </div>
                </li>
            </ul>
            <div class="tab-content" id="pills-tabContent">
                <div class="tab-pane fade show active" id="pills-fleet-tabien" role="tabpanel" aria-labelledby="pills-fleet-tabien-tab">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div id="chart-donut-fleet-category"></div>
                                    <div class="button-container">
                                        <div id="button-fleet-category"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart-bar-fleet-category"></div>
                                    <div class="button-container">
                                        <div id="button-car-category"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="tab-pane fade" id="pills-license" role="tabpanel" aria-labelledby="pills-license-tab">
                    <div class="row">
                        <div class="col-12">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div id="chart-donut-tabien-status"></div>
                                    <div class="button-container">
                                        <div id="button-tabien-status"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart_stacked_bar_tabien"></div>
                                    <div class="button-container">
                                        <div id="button-book-tabien"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart_stacked_bar_other"></div>
                                    <div class="button-container">
                                        <div id="button-book-other"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart_stacked_bar_enter_factory"></div>
                                    <div class="button-container">
                                        <div id="button-book-enter-factory"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="tab-pane fade" id="pills-driver" role="tabpanel" aria-labelledby="pills-driver-tab">
                    <div class="row">
                        <div class="col-12">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div id="chart-donut-driver-fleet"></div>
                                    <div class="button-container">
                                        <div id="button-driver-fleet"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart-bar-dl-category-driver"></div>
                                    <div class="button-container">
                                        <div id="button-dl-category-driver"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart-donut-status-category-driver"></div>
                                    <div class="button-container">
                                        <div id="button-status-category-driver"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="chart-stacked-dl-driver"></div>
                                    <div class="button-container">
                                        <div id="button-dl-driver"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            @*<h2>Fleet Tabien</h2>
        <section>
            
        </section>

        <h2>License</h2>
        <section >
            
        </section>

        <h2>Fleet Driver</h2>
        <section>
            <p>
                Morbi ornare tellus at elit ultrices id dignissim lorem elementum. Sed eget nisl at justo condimentum dapibus. Fusce eros justo,
                pellentesque non euismod ac, rutrum sed quam. Ut non mi tortor. Vestibulum eleifend varius ullamcorper. Aliquam erat volutpat.
                Donec diam massa, porta vel dictum sit amet, iaculis ac massa. Sed elementum dui commodo lectus sollicitudin in auctor mauris
                venenatis.
            </p>
        </section>*@
        </div>
    </div>
    <div id="popup_history"></div>
<script src="~/scripts/Home/dashboard.js"></script>