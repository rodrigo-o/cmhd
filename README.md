# CMHD
Compact representation of Multidimensional data on Hierarchical Domains

This repository is an attemp to implement a generalized form of the CMHD
data structure presented and explainted in this papers:

- https://core.ac.uk/download/pdf/80522508.pdf
- https://dsi.face.ubiobio.cl/mcaniupan/pdfs/SCCC2018CamReady.pdf

# Motivation

This type of structure have a great impact in the way that systems could 
represent and expose diferent views of the same data. In both papers; a 
two-dimensional data structure is discussed with examples; it's 
my aim to implement a succint n-dimensional data structure such that it serves
as open-source documentation of this type of structures and as a dependency for
my detlqav project.

# Roadmap

The following are the succint data structures needed in order to implement the
CMHD.

- [ ] Bitmap
- [ ] Louds
- [ ] K^N Irregular Treap
- [ ] CMHD

# Why F#?

F# is a mature, open source, cross-platform, strongly typed, functional-first 
programming language. It's compact, expressive, explicit and I love the way 
data is represented and queried in it.

# Credits

I'd like to thank the Teams at Database Lab., University of A Coruña, Spain; 
Dept. of Computer Science, University of Chile, Chile: and the University of 
Bio-Bioin, Chile; for they high quality work who inspired me in my searching of 
a better way to do Dimensional querying as a service to use in data driven
applications.

# References

### CMHD
- https://core.ac.uk/download/pdf/80522508.pdf
- https://dsi.face.ubiobio.cl/mcaniupan/pdfs/SCCC2018CamReady.pdf

### K^N Irregular Treaps

- https://core.ac.uk/download/pdf/80522508.pdf (2)
- https://dsi.face.ubiobio.cl/mcaniupan/pdfs/SCCC2017.pdf (k^2 base structure)

### LOUDS

- https://core.ac.uk/download/pdf/80522508.pdf (1)
- https://www.computer.org/csdl/proceedings-article/focs/1989/063533/12OmNx2QUHQ (Download)

### Bitmap

- https://users.dcc.uchile.cl/~gnavarro/algoritmos/ps/wea05.pdf

# License

MIT © Rodrigo Oliveri