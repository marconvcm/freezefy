rm -f ./*.zip
zip -r eclipse_frost-macos.zip Export/macOS
rm -rf ./Export/win64/data_Freezefy_windows_x86_64
zip -r eclipse_frost-win64.zip Export/win64
zip -r eclipse_frost-linux.zip Export/linux

