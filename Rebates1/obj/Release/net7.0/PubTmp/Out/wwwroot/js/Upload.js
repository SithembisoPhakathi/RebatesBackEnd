var ext, ext2, fsize, fi;
            const input = document.querySelector('#filesLate');
            const limit = document.getElementById('limitCount').value;
            // Listen for files selection
            input.addEventListener('change', (e) => {
                // Retrieve all files
                const files = input.files;
                // Check files count
                if (files.length > (10-limit)) {
                    input.value = "";
                    alert('Only ' + (10 - limit) +' files are allowed to uploaded.');
                    return;
                }

                for (let i = 0; i < files.length; i++) {
                    ext = input.files.item(i).name;
                    ext2 = ext.split(".").pop();
                    ext2.toLowerCase();
                    console.log(ext2);

                    switch (ext2) {
                        case 'pdf':
                        case 'jpeg':
                        case 'jpg':
                        case 'png':
                        case 'heif':
                            fsize = input.files.item(i).size;
                            fi = Math.round((fsize / 1024));
                            // The size of the file.
                            if (ext.length > 100) {
                                alert("File name too long.");
                                input.value = '';
                                break;
                            }
                            if (fi >= 10240) {
                                alert("File too Big, please select a file less than 10,2mb");
                                input.value = '';
                            }
                            break;
                        default:
                            alert('File type Not allowed. PDF, JPEG, JPG, PNG,HEIF only.');
                            input.value = '';
                            break;
                    }
                }
            });