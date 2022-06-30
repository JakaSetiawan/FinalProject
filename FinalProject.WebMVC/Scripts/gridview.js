function gridview(id, settings) {
    
    var grid = document.querySelector("#" + id);
    initContent(); 
    var buttonPrev = grid.querySelector("[button-prev]");
    var buttonNext = grid.querySelector("[button-next]");
    var pagelabel = grid.querySelector("[page-count]");
    var pageSize = grid.querySelector("[page-size]");
    var rowCount = grid.querySelector("[row-count]");
    var crtName = grid.querySelector("[criteria-name]");
    var crtValue = grid.querySelector("[criteria-value]"); 
    var selectedRows = [];
    var isCheckedAll = false;
    var columns = settings.columns;
    var option = {
        page: 1,
        pageSize: 5,
        criteria: [],
        order: settings.fieldKey + " ASC"
    };

    function initContent() {
        var templates = '<div class="form-horizontal"> \
                    <div class="form-group"> \
                        <div class="col-md-1 control-label">Kriteria</div> \
                        <div class="col-md-3"> \
                            <select criteria-name class="form-control" required> \
                                <option value="">--Pilih Kriteria--</option> \
                            </select> \
                        </div> \
                        <div class="col-md-2"> \
                            <input criteria-value type="text" name="value" class="form-control" placeholder="kata kunci" required/> \
                        </div> \
                        <div class="col-md-1"> \
                            <button type="button" class="btn btn-primary btn-sm" button-search><i class="fa fa-search"></i> Cari</button> \
                        </div> \
                    </div> \
                    <div class="form-group"> \
                        <div show-criteria class="col-md-10"></div> \
                    </div> \
                    <div class="form-group"><div class="col-md-12"><div selected-info  class="alert alert-warning" style="margin-bottom: 0px;font-size: 1.2em;padding: 10px; display: none; position: relative"><span selected-row>0 row selected</span> <a remove-all style="color: red; cursor: pointer; position: absolute; top: 12px; right: 20px; text-decoration: none"><i class="glyphicon glyphicon-trash"></i> remove</a></div></div></div> \
                </div> \
                <div style="overflow-x: scroll"> \
                    <table class="table table-bordered table-striped"> \
                        <thead> \
                            <tr></tr> \
                        </thead> \
            <tbody></tbody> \
                    </table > \
                </div > \
        <div class="page-info"> \
                    <div style="min-width: 100px; display: inline-block"> \
                        Row(s) Count: <span row-count></span> \
                    </div> \
                    <div style="position: absolute; right: 0; top: 0; min-width: 100px; text-align: right; display: inline-block"> \
                        <span>Page Size: </span> \
                        <div style="display: inline-block; width: 60px; height: 20px;"> \
                            <select page-size class="form-control" style="padding: 4px; height: 30px;"> \
                                <option value="5">5</option> \
                                <option value="10">10</option> \
                                <option value="50">50</option> \
                                <option value="100">100</option>  \
                            </select> \
                        </div> \
                        Page:<span page-count></span> \
                        <button button-prev type="button" class="btn btn-primary btn-sm"><i class="fa fa-chevron-left"></i> Prev</button> \
                        <button button-next type="button" class="btn btn-primary btn-sm">Next <i class="fa fa-chevron-right"></i></button> \
                    </div> \
                </div>';
        grid.innerHTML = templates;
        grid.querySelector("[remove-all]").onclick = function () { 
            if (settings["removeSelected"])settings["removeSelected"](selectedRows);
        };
    }

    function initCriteria() {
        var list = grid.querySelector("[criteria-name]");
        columns.forEach(function (c) {
            if (c.field)
                list.innerHTML += "<option value='" + c.field + "'>" + c.text + "</option>";
        });

        grid.querySelector("[button-search]").onclick = search;
        crtValue.onkeypress = function (e) {
            if (e.keyCode === 13) {
                search();
            }
        };
    }
    
    function initTableHeader() {
        var thead = grid.querySelector("table thead tr");
        if (settings.showCheckbox) {
            thead.innerHTML += "<th><input disabled check-all type='checkbox' /></th>";
        }
        var sort = "";
        Array.prototype.forEach.call(columns, function (c) {
            sort = c.sortable ? "<i class='fa fa-sort'></i>" : "";
            thead.innerHTML += "<th " + (c.sortable ? "sortable" : "") + " field='" + c.field + "'>" + c.text + " " + sort + "</th>";
        });
        var ths = grid.querySelectorAll("th[sortable]");
        Array.prototype.forEach.call(ths, function (th) {
            th.onclick = function (e) {
                var col = th.attributes["field"].value;
                sortColumn(col);
            };
        });

        if (settings.showCheckbox)
        document.querySelector("input[check-all]").onchange = function (e) {
            isCheckedAll = e.target.checked;
            selectedRows = [];
            var checks = document.querySelectorAll("input[check-data]"); 
            Array.prototype.forEach.call(checks, function (v) {
                v.checked = isCheckedAll;
                if (isCheckedAll) {
                    selectedRows.push(v.attributes["check-data"].value);
                }
            });
            var sinfo = grid.querySelector("[selected-info]");
            var slctdRow = grid.querySelector("[selected-row]");
            if (isCheckedAll) { 
                slctdRow.textContent = selectedRows.length + " row(s) selected";
                sinfo.style.display = "block";
            } else {
                slctdRow.textContent = selectedRows.length + " row(s) selected";
                sinfo.style.display = "none";
            }
        };
    }
            
    function loadData() {
        if (settings.showCheckbox) {
            grid.querySelector("[check-all]").checked = false;
        }
        axios.post(settings.url, { page: option})
            .then(function (res) { return res.data; }, function (err) {
                console.log(err);
                buttonPrev.disabled = true;
                buttonNext.disabled = true;
            })
            .then(function (res) {
                var data = res.data;
                var rows = data.Rows || [];
                var tbody = grid.querySelector("table tbody");
                tbody.innerHTML = "";
                rows.forEach(function (r) {
                    var tr = document.createElement("tr");
                    if (settings.showCheckbox) {
                        tr.innerHTML += "<td><input check-data='" + r[settings.fieldKey] + "' type='checkbox' /></td>";
                    }
                    columns.forEach(function (c) {
                        if (c.render) {
                            tr.innerHTML += "<td>" + c.render(r) + "</td>";
                        } else {
                            var className = c.type === "number" ? "class='text-right'" : ""; 
                            tr.innerHTML += "<td " + (c.type === "number" ? className : "" ) + ">" + r[c.field] + "</td>";
                        }
                    });
                    tbody.appendChild(tr);

                    //set action buttons
                    var actions = tr.querySelectorAll("[action]");
                    Array.prototype.forEach.call(actions, function (a) {
                        var action = a.attributes["action"].value;
                        a.onclick = function () { if (settings[action]) settings[action](r); };
                    });

                    //set check event change
                    if (settings.showCheckbox) {
                        var check = tr.querySelector("input[check-data]");
                        //check.checked = selectedRows.indexOf(r[settings.fieldKey]) > -1;
                        var sinfo = grid.querySelector("[selected-info]");
                        var slctdRow = grid.querySelector("[selected-row]");
                        sinfo.style.display = "none";
                        check.onchange = function (e) {
                            var id = e.target.attributes["check-data"].value;
                            var idx = selectedRows.indexOf(id);
                            if (e.target.checked) {
                                selectedRows.push(id); 
                            } else {
                                selectedRows.splice(idx, 1); 
                            } 
                            isCheckedAll = selectedRows.length === data.RowCount;
                            grid.querySelector("[check-all]").checked = isCheckedAll;

                            slctdRow.textContent = selectedRows.length + " row(s) selected";
                            if (selectedRows.length > 0) { 
                                sinfo.style.display = "block";
                            } else {
                                sinfo.style.display = "none";
                            }
                        };

                    }
                });

                //set attributes of paging
                if (settings.showCheckbox) {
                    selectedRows = [];
                    grid.querySelector("[check-all]").disabled = (rows.length !== data.RowCount);
                }
                rowCount.textContent = data.RowCount;
                pagelabel.textContent = option.page + " / " + data.PageCount;
                buttonPrev.disabled = option.page === 1;
                buttonNext.disabled = option.page === data.PageCount;
            }, function (err) {
                console.log(err);
                buttonPrev.disabled = true;
                buttonNext.disabled = true;
            });
    }
    
    function showCriteria() {
        var el = grid.querySelector("[show-criteria]");
        el.innerHTML = "";

        option.criteria.forEach(function (c) {
            var name = c.criteria.split(/(?=[A-Z])/).join(" ");
            el.innerHTML += "<div class='item-search'>Search <span> " + name + "</span> contains <span> '" + c.value + "'</span><a>remove</a></div >";

        });

        var removes = el.querySelectorAll("a");
        Array.prototype.forEach.call(removes, function (a, i) {
            a.onclick = function () {
                option.criteria.splice(i, 1);
                showCriteria();
                loadData();
            };
        });
    }

    function search() {
        if (crtName.checkValidity() && crtValue.checkValidity()) {
            option.criteria.push({ criteria: crtName.value, value: crtValue.value });
            option.page = 1;
            loadData();
            showCriteria();
            crtName.selectedIndex = 0;
            crtValue.value = "";
        }
    }

    function sortColumn(col) {
        var orders = option.order.split(" ");
        if (orders[0] === col) {
            option.order = orders[0] + " " + (orders[1] === "ASC" ? "DESC" : "ASC");
        } else {
            option.order = col + " ASC"; 
        }
        loadData();
        orders = option.order.split(" ");
        var ths = grid.querySelectorAll("th[sortable]");
        Array.prototype.forEach.call(ths, function (th) {
            var i = th.querySelector("i");
            if (th.attributes["field"].value === orders[0]) {
                i.className = orders[1] === "ASC" ? "glyphicon glyphicon-sort-by-attributes" : "glyphicon glyphicon-sort-by-attributes-alt";
                i.style.opacity = "0.8";
                i.style.color = "darkBlue";
                th.style.background = "#e8e5ef";
            } else {
                i.className = "fa fa-sort";
                i.style.opacity = "0.4";
                th.style.background = "white";
            }
        });
    }

    initCriteria();
    initTableHeader();
    if (settings.displayData === false) {
        //disable auto display Data
    } else {
        loadData();
    }

    pageSize.value = option.pageSize;
    pageSize.onchange = function (e) {
        option.page = 1;
        option.pageSize = e.target.value;
        loadData();
    };
    buttonPrev.onclick = function () {
        option.page--;
        loadData();
    };
    buttonNext.onclick = function () {
        option.page++;
        loadData();
    };
    
    return {
        reload: function () {
            loadData(); 
        }
    };
}