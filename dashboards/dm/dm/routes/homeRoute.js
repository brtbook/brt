var express = require('express');
var router = express.Router();

router.get( '/', function ( req, res )
{
    res.render( 'home', { title: 'Business in Real-Time' });
});

module.exports = router;