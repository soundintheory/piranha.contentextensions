/*global
    piranha
*/

piranha.contentextensions = piranha.contentextensions || {};
piranha.contentextensions.permissions = {
    loaded: false,

    load: function () {
        if (!this.loaded) {
            return fetch(piranha.baseUrl + "manager/api/contentextensions/singletons/permissions")
                .then((response) => response.json())
                .then((result) => {
                    this.loaded = true;
                    return result;
                })
                .catch(function (error) {
                    console.log("error:", error);
                });
        } else {
            return Promise.resolve();
        }
    }
};