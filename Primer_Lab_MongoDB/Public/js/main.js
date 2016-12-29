var jq = $(document);

var person = function () {
    // private scope

    // public scope
    return {

        creteTableStructure: function creteTableStructure() {
            var $table = jq.find('#tblPeople');
            $table.bootstrapTable('destroy');
            $table.bootstrapTable({
                data: [],
                cache: false,
                height: 494,
                striped: false,
                pagination: false,
                sortable: true,
                sortOrder: 'asc',
                showToggle: false,
                showPaginationSwitch: false,
                pageSize: 10,
                pageList: [10, 20, 30, 150, 250, 350, 500],
                search: true,
                showColumns: false,
                showRefresh: false,
                minimumCountColumns: 2,
                rowAttributes: function (row, index) { return { 'data-guid': row.Guid }; },
                columns: [{
                    field: 'Name',
                    title: 'Name',
                    sortable: true
                }, {
                    field: 'Age',
                    title: 'Age',
                    sortable: true
                }]
            });
        }
        , setInfoInInputs: function setInfoInInputs() {

            var guid = jq.find('#tblPeople tbody tr.warning').data('guid');
            var req = $.ajax({ url: 'Home/findDoc', type: 'POST', async: false, data: { pGuid: guid } }).responseText;

            try {
                req = JSON.parse(req);
            } catch (e) { req = []; }

            jq.find('input[name="txtGuid"]').val(req.Guid);
            jq.find('input[name="txtName"]').val(req.Name);
            jq.find('input[name="txtAge"]').val(req.Age);

        }
        , getPeople: function getPeople() {
            var items = [];
            var req = $.ajax({ url: 'Home/getDocs', type: 'POST', async: false }).responseText;
            try { items = JSON.parse(req); } catch (err) { items = []; }
            return items;
        }
        , addPerson: function addPerson(user, age) {
            if (user == '')
                return 'Please fill the name.';
            if (age == '')
                return 'Please fill the age.';
            if (isNaN(age))
                return 'The age entered must be a number.';

            if (parseInt(age) < 1)
                return 'The age entered must be a equal or greater than 1.';

            var req = $.ajax({ url: 'Home/addDoc', type: 'POST', async: false, data: { pName: user, pAge: age } }).responseText;
            return req;
        }
        , modifyPerson: function modifyPerson(user, age, guid) {
            if (user == '')
                return 'Please fill the name.';
            if (age == '')
                return 'Please fill the age.';
            if (isNaN(age))
                return 'The age entered must be a number.';

            if (parseInt(age) < 1)
                return 'The age entered must be a equal or greater than 1.';

            if (guid == '')
                return "The information to search doesn't exist.";

            var req = $.ajax({ url: 'Home/modifyDoc', type: 'POST', async: false, data: { pName: user, pAge: age, pGuid: guid } }).responseText;
            return req;
        }
        , deletePerson: function deletePerson(guid) {
           
            if (guid == '')
                return "The information to search doesn't exist.";

            var req = $.ajax({ url: 'Home/deleteDoc', type: 'POST', async: false, data: { pGuid: guid } }).responseText;
            return req;
        }
        , reLoadTable: function reloadTable($table, data) {
            data = data || [];
            $table.bootstrapTable('load', data);
        }

    };

};

var Person = person();

jq.ready(function () {

    Person.creteTableStructure();
    var $table = jq.find('#tblPeople');
    var data = Person.getPeople();
    Person.reLoadTable($table, data);

});

jq.find('#btnAdd').on('click', function () {

    var name = jq.find('input[name="txtName"]').val();
    var age = jq.find('input[name="txtAge"]').val();

    Person.addPerson(name, age);

    var $table = jq.find('#tblPeople');
    var data = Person.getPeople();

    Person.reLoadTable($table, data);

});

jq.find('#btnUpdate').on('click', function () {

    var name = jq.find('input[name="txtName"]').val();
    var age = jq.find('input[name="txtAge"]').val();
    var guid = jq.find('input[name="txtGuid"]').val();

    Person.modifyPerson(name, age, guid);

    var $table = jq.find('#tblPeople');
    var data = Person.getPeople();

    Person.reLoadTable($table, data);

});

jq.find('#btnDelete').on('click', function () {

    var guid = jq.find('input[name="txtGuid"]').val();

    Person.deletePerson(guid);

    var $table = jq.find('#tblPeople');
    var data = Person.getPeople();

    Person.reLoadTable($table, data);

});

jq.on('click', '#tblPeople tbody tr', function () {

    jq.find('#tblPeople tbody tr.warning').removeClass('warning');
    var $selectedRow = $(this);
    $selectedRow.addClass('warning');
    Person.setInfoInInputs();

});