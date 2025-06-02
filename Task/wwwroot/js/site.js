function fillSelectors(referenceArray, selectorData, selectedSelector, isSizeSelector) {
    let returnHTML = '';
    referenceArray.forEach((item) => {
        if (selectorData.includes(item)) {
            const activeClass = (item === selectedSelector) ? 'active' : '';
            returnHTML += `<span class="size ${activeClass}">${item} ${isSizeSelector ? 'см' : ''}</span>`;
        }
        else {
            returnHTML += `<span class="size disable">${item} ${isSizeSelector ? 'см' : ''}</span>`;;
        }
    })
    return returnHTML;
}

function getAvaiableSizes() {
    return $.ajax({
        url: '/api/pizza/sizes',
        method: 'GET',
        dataType: 'json'
    });
}

function getAvaiableTypes() {
    return $.ajax({
        url: '/api/pizza/types',
        method: 'GET',
        dataType: 'json'
    });
}

async function getActiveSelectors(pizza) {
    console.log(pizza);
    const avaiableSizes = await getAvaiableSizes();
    const avaiableTypes = await getAvaiableTypes();
    const activeSize = pizza.sizes.includes(avaiableSizes[0])
        ? avaiableSizes[0]
        : pizza.sizes.find(size => avaiableSizes.includes(size));
    let sizesHTML = fillSelectors(avaiableSizes, pizza.sizes, activeSize, true);
    let typesHTML = '';
    if (pizza.types.length > 0) {
        const activeType = pizza.types.includes(avaiableTypes[0])
            ? avaiableTypes[0]
            : pizza.types.find(type => avaiableTypes.includes(type))
        typesHTML = fillSelectors(avaiableTypes, pizza.types, activeType, false)
    }
    return { sizes: sizesHTML, types: typesHTML }
}
async function renderPizzaCard(pizza) {
    const selectorsHTML = await getActiveSelectors(pizza);
    const hitHTML = pizza.isHit
        ? '<span class="pizza-card-hit">HIT</span>'
        : '';
    let halfOptionHTML = '';
    if (pizza.showHalf) {
        pizza.canHalf
            ? halfOptionHTML += '<span class="pizza-card-half-selector active">1/2</span>'
            : halfOptionHTML += '<span class="pizza-card-half-selector disable">1/2</span>'
    }
    const card = `
            <div class="pizza-card" data-id=${pizza.id}>
                <div class="pizza-card-header">
                    ${hitHTML}
                    <img class="pizza-card-img"
                         src="${pizza.image}" alt=${pizza.name}/>
                    <span class="pizza-card-name">
                            ${pizza.name}
                    </span>
                    <div class="pizza-card-buttons">
                        <span class="pizza-card-edit" onclick="openEditPizzaModal(${pizza.id})">
                            <i class="bi bi-pencil"></i>
                        </span>
                        <span class="pizza-card-delete">
                            <form action="/Home/Delete" method="post" class="delete-form">
                                <input type="hidden" name="Id" value="${pizza.id}" />
                                <input type="hidden" name="returnUrl" value="${window.location.pathname}" />
                                <button type="submit" class="btn btn-danger delete-btn">
                                    <i class="bi bi-x-circle"></i>
                                </button>
                            </form>
                        </span>
                    </div>
                </div>
                <div class="pizza-card-description">
                    ${pizza.description}
                </div>
                <div class="pizza-card-selectors">
                    <div class="size-selector">
                        ${selectorsHTML.sizes}
                    </div>
                    <div class="type-selector">
                        ${selectorsHTML.types}
                    </div>
                </div>
                <div class="pizza-card-info">
                    <span class="pizza-card-price">${pizza.price} Р</span>
                    <span class="pizza-card-weight">${pizza.weight} гр</span>
                    <span class="pizza-card-half">${halfOptionHTML}</span>
                </div>
                <button class="in-cart-button">В корзину</button>
            </div>
        `;
    return card;
}
async function renderModalPizzaCard(pizza) {
    pizza = JSON.parse(pizza);
    const selectorsHTML = await getActiveSelectors(pizza);
    const hitHTML = pizza.isHit
        ? '<span class="pizza-card-hit">HIT</span>'
        : '';
    let halfOptionHTML = '';
    if (pizza.showHalf) {
        pizza.canHalf
            ? halfOptionHTML += '<span class="pizza-card-half-selector active">1/2</span>'
            : halfOptionHTML += '<span class="pizza-card-half-selector disable">1/2</span>'
    }
    card = `
    <div class="modal-content">
        <div class="modal-header">
            <h5 class="modal-title">${pizza.name}</h5>
        </div>
        <div class="modal-body">
            <div class="pizza-card-modal" data-id=${pizza.id}>
                <div class="pizza-img-container">
                    <img class="pizza-card-img" src="${pizza.image}" alt="${pizza.name}" />
                </div>
                <div class="pizza-body-modal">
                    <div class="pizza-card-header">
                        ${hitHTML}
                        <span class="pizza-card-name">${pizza.name}</span>
                    </div>
                    <div class="pizza-card-description">
                        ${pizza.description}
                    </div>

                    <div class="pizza-card-selectors">
                        <div class="size-selector">
                           ${selectorsHTML.sizes}
                        </div>
                        <div class="type-selector">
                            ${selectorsHTML.types}
                        </div>
                    </div>

                    <div class="pizza-card-info">
                        <span class="pizza-card-price">${pizza.price} Р</span>
                        <span class="pizza-card-weight">${pizza.weight} гр</span>
                        <span class="pizza-card-half">
                            ${halfOptionHTML}
                        </span>
                    </div>
                    <button class="in-cart-button">В корзину</button>
                </div>
            </div>
        </div>
    </div>
    `;
    return card;
}
function setRedirectToModalOnAllPizzas() {
    $(document).on('click', '.pizza-card-img, .pizza-card-name', function () {
        const parent = $(this).closest('.pizza-card');
        const pizzaId = parent.data('id');
        if (pizzaId) {
            $.ajax({
                url: `/api/pizza/${pizzaId}`,
                type: "GET",
                data: { id: pizzaId },
                dataType: "html",
                success: async function (pizza) {
                    const pizzaHtml = await renderModalPizzaCard(pizza);
                    $('#pizzaModal .modal-dialog').html(pizzaHtml);
                    $('#pizzaModal').modal('show');
                },
                error: function (data) {
                    $('#pizzaModal .modal-dialog').html('<div class="alert alert-danger">Ошибка загрузки данных</div>');
                    $('#pizzaModal').modal('show');
                }
            });

            $('#pizzaModal').on('hidden.bs.modal', function () {
                $(this).find('.modal-dialog').empty();
            });
        }
        else {
            console.log(parent, "не имеет атрибут data-id");
            $('#pizzaModal .modal-dialog').html('<div class="alert alert-danger">Ошибка загрузки данных</div>');
            $('#pizzaModal').modal('show');
        }
    });
}

function handleFormModal(mode, pizzaId = null) {
    const isEdit = mode === 'edit';
    const url = isEdit ? '/Home/Edit' : '/Home/Create';
    const apiUrl = isEdit ? `/api/pizza/${pizzaId}` : '/api/pizza';
    const httpMethod = isEdit ? 'PUT' : 'POST';
    const requestData = isEdit ? { id: pizzaId } : {};

    $.ajax({
        url: url,
        type: 'GET',
        data: requestData,
        success: function (html) {
            $('#pizzaModal .modal-dialog').html(html);
            $('#pizzaModal').modal('show');
            $('#pizzaModal').find('input[name="returnUrl"]').val(window.location.href);

            $('#pizzaModal').off('submit').on('submit', 'form', function (e) {
                e.preventDefault();
                const $form = $(this);

                const data = {
                    Id: Number($form.find('[name="Id"]').val()) || 0,
                    Name: $form.find('[name="Name"]').val(),
                    Description: $form.find('[name="Description"]').val(),
                    Image: $form.find('[name="Image"]').val(),
                    Price: Number($form.find('[name="Price"]').val()) || 0,
                    Weight: Number($form.find('[name="Weight"]').val()) || 0,
                    CanHalf: $form.find('[name="CanHalf"]').is(':checked'),
                    ShowHalf: $form.find('[name="ShowHalf"]').is(':checked'),
                    IsHit: $form.find('[name="IsHit"]').is(':checked'),
                    Sizes: $form.find('[name="Sizes"]:checked').map(function () {
                        return parseInt(this.value);
                    }).get(),
                    Types: $form.find('[name="Types"]:checked').map(function () {
                        return this.value;
                    }).get()
                };

                $.ajax({
                    url: apiUrl,
                    type: httpMethod,
                    contentType: 'application/json',
                    data: JSON.stringify(data),
                    success: function () {
                        $('#pizzaModal').modal('hide');
                        location.reload();
                    },
                    error: function (xhr) {
                        if (xhr.status === 400 && xhr.responseJSON && xhr.responseJSON.errors) {
                            $form.find('.text-danger').text('');
                            const errors = xhr.responseJSON.errors;
                            for (const key in errors) {
                                $form.find(`[data-valmsg-for="${key}"]`).text(errors[key]);
                            }
                        } else {
                            $('#pizzaModal .modal-dialog').html('<div class="alert alert-danger">Ошибка отправки формы</div>');
                        }
                    }
                });
            });
        },
        error: function () {
            $('#pizzaModal .modal-dialog').html('<div class="alert alert-danger">Ошибка загрузки формы</div>');
            $('#pizzaModal').modal('show');
        }
    });

    $('#pizzaModal').on('hidden.bs.modal', function () {
        $(this).find('.modal-dialog').empty();
    });
}

function openEditPizzaModal(pizzaId) {
    handleFormModal('edit', pizzaId);
}

function openCreatePizzaModal() {
    handleFormModal('create');
}


function setConfrimOnDelete() {
    $(document).on('click', '.delete-btn', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var pizzaName = $(this).closest('.pizza-card').find('.pizza-card-name').text();
        var form = $(this).closest('.delete-form');

        if (confirm(`Точно удалить "${pizzaName.trim()}"?`)) {
            form.submit();
        }
        return false;
    });
}